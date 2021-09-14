package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.strategy.*;
import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.*;
import logger.EventLogger;

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
                    return;
                }
                //Fetch the request from the client
                BufferedReader reader = connection.getReader();
                String requestData = reader.readLine();

                if (requestData == null) {
                    continue;
                }

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
                        }
                        return;
                    }
                    request = new RequestDecoder().deserialize(element, null, null);

                    //Process request based on analysis type
                    if (request.getRequestType().contains("Analyze")) {
                        EventLogger.getLogger().info("Performing analysis on uploaded media");

                        new StoredMediaAnalysisStrategy().processRequest(request, informationHolder, connectionHandler, connection.getConnectionId());
                    } else {
                        EventLogger.getLogger().info("Performing live analysis request");
                        new LiveAnalysisStrategy().processRequest(request, informationHolder, connectionHandler, connection.getUserId());
                    }
                } catch (Exception e) {
                    EventLogger.getLogger().logException(e);
                    out.append(e.getMessage()).flush();
                }
            } catch (SocketException socketException) {
                EventLogger.getLogger().info("Client has disconnected");
                return;
            } catch (IOException exception) {
                EventLogger.getLogger().logException(exception);
                return;
            }
        }
    }
}
