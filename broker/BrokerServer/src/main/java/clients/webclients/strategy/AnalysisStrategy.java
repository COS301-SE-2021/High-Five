package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

import java.io.*;

/**
 * Interface to define a strategy to handle analysis requests.
 */
public interface AnalysisStrategy {

    /**
     * Processes the request.
     * @param request Request to process
     * @param information Server to use for analysis
     * @param writer Writer to get any responses from the server
     */
    void processRequest(AnalysisRequest request, ServerInformation information, BufferedWriter writer) throws IOException;
}
