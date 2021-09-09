package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.serverinfo.ServerUsage;

import java.io.*;
import java.util.Map;

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
    void processRequest(AnalysisRequest request, ServerInformationHolder information, BufferedWriter writer) throws IOException;
}
