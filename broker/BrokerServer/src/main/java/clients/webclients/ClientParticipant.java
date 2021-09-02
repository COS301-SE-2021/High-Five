package clients.webclients;

import clients.webclients.strategy.LiveAnalysisStrategy;
import clients.webclients.strategy.VideoAnalysisStrategy;
import dataclasses.clientrequest.ClientRequest;
import dataclasses.clientrequest.codecs.RequestDecoder;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import org.apache.commons.io.IOUtil;

import javax.websocket.DecodeException;
import java.io.*;
import java.net.Socket;

public class ClientParticipant extends WebClient{

    private Socket connection;
    private final ServerInformationHolder informationHolder;

    public ClientParticipant(Socket connection, ServerInformationHolder informationHolder) {
        this.informationHolder = informationHolder;
        this.connection = connection;
    }

    @Override
    public boolean heartbeat() {
        return false;
    }

    @Override
    public void listen() throws InterruptedException {
        ServerInformation info = informationHolder.get();

        try {
            StringWriter requestDataWriter = new StringWriter();
            IOUtil.copy(connection.getInputStream(), requestDataWriter, "UTF-8");
            String requestData = requestDataWriter.toString();

            Writer out = new BufferedWriter(new OutputStreamWriter(
                    connection.getOutputStream()));

            try {
                ClientRequest request = new RequestDecoder().decode(requestData);

                if (request.getResourceType().equals("live")) {
                    new LiveAnalysisStrategy().processRequest(request, info, out);
                } else {
                    new VideoAnalysisStrategy().processRequest(request, info, out);
                }
            } catch (DecodeException e) {
                out.append(e.getMessage()).flush();
            }

            connection.close();
        } catch (IOException ignored) {}
    }
}
