package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

import java.io.IOException;
import java.io.Writer;

public interface AnalysisStrategy {
    void processRequest(AnalysisRequest request, ServerInformation information, Writer writer) throws IOException;
}