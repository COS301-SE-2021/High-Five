package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.ClientRequest;

import java.io.IOException;
import java.io.Writer;

public class LiveAnalysisStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(ClientRequest request, ServerInformation information, Writer writer) throws IOException {

        String infoString;
        if (information == null) {
            infoString = "No servers are available";
        } else {
            infoString = information.toString();
        }

        writer.append(infoString).flush();

    }
}
