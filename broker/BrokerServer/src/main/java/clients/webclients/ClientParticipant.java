package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.strategy.*;
import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.*;
import logger.EventLogger;

import java.io.*;

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
        ServerInformation info = informationHolder.get();
        try {
            //Fetch the request from the client
            StringWriter requestDataWriter = new StringWriter();
            connection.setInputWriter(requestDataWriter);
            String requestData = requestDataWriter.toString();

            //Response object for sending a response to the client
            Writer out = connection.getOutputWriter();

            try {
                //Decodes the JSON message
                EventLogger.getLogger().info("Decoding request from client");
                AnalysisRequest request;
                JsonElement element = new Gson().fromJson(requestData, JsonElement.class);
                request = new RequestDecoder().deserialize(element, null,null);

                //Process request based on analysis type
                if (request.getAnalysisType().equals("live")) {
                    EventLogger.getLogger().info("Performing live analysis request");
                    EventLogger.getLogger().debug(requestData);
                    new LiveAnalysisStrategy().processRequest(request, info, out);
                } else {
                    EventLogger.getLogger().info("Performing analysis on uploaded media");
                    new VideoAnalysisStrategy().processRequest(request, info, out);
                }
            } catch (Exception e) {
                out.append(e.getMessage()).flush();
                EventLogger.getLogger().error(e.getMessage());
            }

            connection.close();
        } catch (IOException exception) {
            EventLogger.getLogger().error(exception.getMessage());
        }
    }
}
