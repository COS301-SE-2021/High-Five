package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

import java.io.BufferedWriter;
import java.io.IOException;
import java.io.Writer;

public class LiveAnalysisStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformation information, BufferedWriter writer) throws IOException {

        String infoString;
        if (information == null) {
            infoString = "No servers are available";
        } else {
            infoString = information.toString();
        }

        writer.append(infoString).flush();

    }
}
