package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import clients.webclients.strategy.*;
import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.RequestQueueItem;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.*;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.impl.crypto.DefaultJwtSignatureValidator;
import logger.EventLogger;
import managers.requestqueue.RequestQueue;

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
    private static final int MAX_NULL_COUNT = 10;
    private int nullCounter = 0;
    private volatile boolean isRunning = true;

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
        while (isRunning) {
            try {
                if (connection.isClosed()) {
                    commitSuicide();
                    return;
                }
                //Fetch the request from the client
                BufferedReader reader = connection.getReader();
                String requestData = reader.readLine();

                if (requestData == null) {
                    Thread.sleep(1000L);
                    if (++nullCounter == MAX_NULL_COUNT) {
                        commitSuicide();
                        return;
                    }
                    continue;
                }

                //If the client closes the connection, tell the connection handler
                //to remove the connection and exit this function
                if (requestData.length() >= CLOSECONNECTION.length() &&
                        requestData.substring(0, CLOSECONNECTION.length()).contains(CLOSECONNECTION)) {
                    commitSuicide();
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
                        if (connection.isClosed() || ++nullCounter == MAX_NULL_COUNT) {
                            commitSuicide();
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
                        commitSuicide();
                        return;
                    }

                    //When a client asks to synchronise, then send and acknowledgement
                    if (request.getRequestType().contains(SYN)) {
                        EventLogger.getLogger().info("Synchronising with client " + connection.getUserId());
                        ResponseObject responseObject = new ResponseObject("none", null, "ACK", connection.getConnectionId());
                        connectionHandler.onNext(responseObject);
                        Thread.sleep(1000);
                        continue;
                    }

                    RequestQueue.getInstance().addToQueue(new RequestQueueItem(request, informationHolder, connection, connectionHandler,this));


                } catch (Exception e) {
                    EventLogger.getLogger().logException(e);
                    out.append(e.getMessage()).flush();
                    commitSuicide();
                    return;
                }
            } catch (IOException | NullPointerException socketException) {
                EventLogger.getLogger().logException(socketException);
                commitSuicide();
                return;
            }
            Thread.sleep(1000);
        }
        commitSuicide();
    }

    @Override
    public void terminate() {
        isRunning = false;
    }

    private void commitSuicide() {
        EventLogger.getLogger().info("Client has disconnected");
        try {
            connection.close();
        } catch (Exception e) {
            EventLogger.getLogger().logException(e);
        }
        connectionHandler.removeConnection(connection.getConnectionId());
    }
}
