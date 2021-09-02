package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.ClientRequest;

import java.io.IOException;
import java.io.Writer;

public interface AnalysisStrategy {
    void processRequest(ClientRequest request, ServerInformation information, Writer writer) throws IOException;
}
