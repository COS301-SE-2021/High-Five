package clients.webclients;

import clients.webclients.connection.Connection;
import clients.webclients.strategy.LiveAnalysisStrategy;
import clients.webclients.strategy.VideoAnalysisStrategy;
import com.google.gson.Gson;
import com.google.gson.JsonElement;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.serverinfo.codecs.ServerInformationDecoder;
import org.apache.commons.io.IOUtil;

import javax.websocket.DecodeException;
import java.io.*;
import java.net.Socket;

public class ClientParticipant extends WebClient{

    private Connection connection;
    private final ServerInformationHolder informationHolder;

    public ClientParticipant(Connection connection, ServerInformationHolder informationHolder) {
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
                AnalysisRequest request;
                JsonElement element = new Gson().fromJson(requestData, JsonElement.class);
                request = new RequestDecoder().deserialize(element, null,null);

                //Process request based on analysis type
                if (request.getAnalysisType().equals("live")) {
                    new LiveAnalysisStrategy().processRequest(request, info, out);
                } else {
                    new VideoAnalysisStrategy().processRequest(request, info, out);
                }
            } catch (Exception e) {
                out.append(e.getMessage()).flush();
            }

            connection.close();
        } catch (IOException ignored) {}
    }
}
