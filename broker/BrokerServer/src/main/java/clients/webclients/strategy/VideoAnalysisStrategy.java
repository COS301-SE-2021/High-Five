package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;

import java.net.Socket;

public class VideoAnalysisStrategy implements AnalysisStrategy{

    /**
     * Sends an instruction to an analysis server to process a file specified by the client.
     * @param connection
     * @param information
     */
    @Override
    public void processRequest(Socket connection, ServerInformation information) {

    }
}
