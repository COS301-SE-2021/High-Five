package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

import java.io.*;

public class VideoAnalysisStrategy implements AnalysisStrategy{

    /**
     * Sends an instruction to an analysis server to process a file specified by the client.
     * @param request
     * @param information
     */
    @Override
    public void processRequest(AnalysisRequest request, ServerInformation information, Writer writer) throws IOException {
        Runtime rt = Runtime.getRuntime();
        Process proc = rt.exec(System.getenv("KAFKA_SEND_COMMAND"));


        String line = null;

        BufferedWriter outputStreamWriter =
                new BufferedWriter(new OutputStreamWriter(proc.getOutputStream()));
    }
}
