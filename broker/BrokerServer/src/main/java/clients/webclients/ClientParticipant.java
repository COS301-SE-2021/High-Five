package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import clients.webclients.strategy.*;
import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.*;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.impl.crypto.DefaultJwtSignatureValidator;
import logger.EventLogger;

import javax.crypto.spec.SecretKeySpec;
import java.io.*;
import java.net.SocketException;

/**
 * Client participant class that fetches server information (the server with the least usage),
 * determines what kind of analysis to run, performs the necessary action and informs the
 * client of the outcome.
 */
public class ClientParticipant extends WebClient{

    private final Connection connection;
    private final ConnectionHandler connectionHandler;
    private final ServerInformationHolder informationHolder;
    private static final String CLOSECONNECTION = "closeconnection";
    public static final String SYN = "Syn";

    public ClientParticipant(Connection connection, ConnectionHandler handler, ServerInformationHolder informationHolder) {
        EventLogger.getLogger().info("Starting ClientParticipant session");
        this.informationHolder = informationHolder;
        this.connection = connection;
        this.connectionHandler = handler;
    }

    @Override
    public boolean heartbeat() {
        return false;
    }

    /**
     * Reads a request from the client and processes the request.
     */
    @Override
    public void listen() throws InterruptedException {
        while (true) {
            try {
                if (connection.isClosed()) {
                    EventLogger.getLogger().info("Client has disconnected");
                    connection.close();
                    connectionHandler.removeConnection(connection.getConnectionId());
                    return;
                }
                //Fetch the request from the client
                BufferedReader reader = connection.getReader();
                String requestData = reader.readLine();

                if (requestData == null) {
                    continue;
                }

                //If the client closes the connection, tell the connection handler
                //to remove the connection and exit this function
                if (requestData.length() >= CLOSECONNECTION.length() &&
                        requestData.substring(0, CLOSECONNECTION.length()).contains(CLOSECONNECTION)) {
                    EventLogger.getLogger().info("Client has disconnected");
                    connection.close();
                    connectionHandler.removeConnection(connection.getConnectionId());
                    return;
                }

                //Response object for sending a response to the client
                BufferedWriter out = connection.getWriter();

                try {
                    //Decodes the JSON message
                    EventLogger.getLogger().info("Decoding request from client");
                    AnalysisRequest request;
                    JsonElement element = new Gson().fromJson(requestData, JsonElement.class);
                    if (element == null) {
                        if (connection.isClosed()) {
                            EventLogger.getLogger().info("Client has disconnected");
                            connection.close();
                            connectionHandler.removeConnection(connection.getConnectionId());
                        }
                        return;
                    }

                    //verifying that the request came from the correct backend.
                    request = new RequestDecoder().deserialize(element, null, null);
                    String secretKey = System.getenv("BROKER_SECRET");

                    String[] chunks = request.getAuthorization().split("\\.");

                    String tokenWithoutSignature = chunks[0] + "." + chunks[1];
                    String signature = chunks[2];

                    SignatureAlgorithm sa = SignatureAlgorithm.HS256;
                    SecretKeySpec secretKeySpec = new SecretKeySpec(secretKey.getBytes(), sa.getJcaName());

                    DefaultJwtSignatureValidator validator = new DefaultJwtSignatureValidator(sa, secretKeySpec);

                    if (!validator.isValid(tokenWithoutSignature, signature)) {
                        EventLogger.getLogger().warn("Client " + connection.getUserId() + " could not be verified");
                        String response = "{\"status\":\"error\",\"reason\":\"Could not verify JWT token!\"}";
                        ResponseObject responseObject = new ResponseObject(request.getRequestType(), null, response, connection.getConnectionId());
                        connectionHandler.onNext(responseObject);
                        return;
                    }

                    //When a client asks to synchronise, then send and acknowledgement
                    if (request.getRequestType().contains(SYN)) {
                        EventLogger.getLogger().info("Synchronising with client " + connection.getUserId());
                        ResponseObject responseObject = new ResponseObject("none", null, "ACK", connection.getConnectionId());
                        connectionHandler.onNext(responseObject);
                        continue;
                    }

                    //Process request based on analysis type
                    if (request.getRequestType().contains("Analyze")) {
                        EventLogger.getLogger().info("Performing analysis on uploaded media");
                        new StoredMediaAnalysisStrategy().processRequest(request, informationHolder, connectionHandler, connection.getConnectionId());
                    } else if (request.getRequestType().contains("StartLiveAnalysis")){
                        EventLogger.getLogger().info("Performing live analysis request");
                        new LiveAnalysisStrategy().processRequest(request, informationHolder, connectionHandler, connection.getConnectionId());
                    } else {
                        EventLogger.getLogger().info("Performing live stream request");
                        new LiveAnalysisStrategy().processRequest(request, informationHolder, connectionHandler, connection.getConnectionId());
                    }

                } catch (Exception e) {
                    EventLogger.getLogger().logException(e);
                    out.append(e.getMessage()).flush();
                }
            } catch (SocketException socketException) {
                EventLogger.getLogger().info("Client has disconnected");
                try {
                    connection.close();
                } catch (IOException e) {
                    EventLogger.getLogger().logException(e);
                }
                connectionHandler.removeConnection(connection.getConnectionId());
                return;
            } catch (IOException | NullPointerException exception) {
                EventLogger.getLogger().logException(exception);
                try {
                    connection.close();
                } catch (IOException e) {
                    EventLogger.getLogger().logException(e);
                }
                connectionHandler.removeConnection(connection.getConnectionId());
                return;
            }
        }
    }
}
