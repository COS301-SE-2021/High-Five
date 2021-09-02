package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;

import java.net.Socket;

public interface AnalysisStrategy {
    void processRequest(Socket connection, ServerInformation information);
}
