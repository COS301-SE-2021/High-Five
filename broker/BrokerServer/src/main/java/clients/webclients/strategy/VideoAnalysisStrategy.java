package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

import java.io.Writer;

public class VideoAnalysisStrategy implements AnalysisStrategy{

    /**
     * Sends an instruction to an analysis server to process a file specified by the client.
     * @param request
     * @param information
     */
    @Override
    public void processRequest(AnalysisRequest request, ServerInformation information, Writer writer) {

    }
}
