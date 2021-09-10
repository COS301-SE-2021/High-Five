package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.strategy.*;
import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.*;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import logger.EventLogger;

import java.io.*;
import java.util.HashMap;
import java.util.Map;

/**
 * Client participant class that fetches server information (the server with the least usage),
 * determines what kind of analysis to run, performs the necessary action and informs the
 * client of the outcome.
 */
public class ClientParticipant extends WebClient{

    private Connection connection;
    private final ServerInformationHolder informationHolder;

    public ClientParticipant(Connection connection, ServerInformationHolder informationHolder) {
        EventLogger.getLogger().info("Starting ClientParticipant session");
        this.informationHolder = informationHolder;
        this.connection = connection;
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
        if (!connection.isConnected()) {
            return;
        }
        try {
            //Fetch the request from the client
            BufferedReader reader = connection.getReader();
            String requestData = reader.readLine();

            //Response object for sending a response to the client
            BufferedWriter out = connection.getWriter();

            try {
                //Decodes the JSON message
                EventLogger.getLogger().info("Decoding request from client");
                AnalysisRequest request;
                JsonElement element = new Gson().fromJson(requestData, JsonElement.class);
                request = new RequestDecoder().deserialize(element, null,null);

                //Process request based on analysis type
                if (request.getRequestType().contains("Analyze")) {
                    EventLogger.getLogger().info("Performing analysis on uploaded media");

                    new StoredMediaAnalysisStrategy().processRequest(request, informationHolder, out);
                } else {
                    EventLogger.getLogger().info("Performing live analysis request");
                    new LiveAnalysisStrategy().processRequest(request, informationHolder, out);
                }
            } catch (Exception e) {
                out.append(e.getMessage()).flush();
                EventLogger.getLogger().error(e.getMessage());
            }
        } catch (IOException exception) {
            EventLogger.getLogger().error(exception.getMessage());
        }
    }
}
