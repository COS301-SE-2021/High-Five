package clients.webclients.strategy;

import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import logger.EventLogger;

import java.io.*;

public class VideoAnalysisStrategy implements AnalysisStrategy{

    /**
     * Sends an instruction to an analysis server to process a file specified by the client.
     *
     * @param request Analysis request for media to be analysed
     * @param information Information about the server to perform analysis.
     * @param writer Writer to inform web client that media is going to be analysed
     */
    @Override
    public void processRequest(AnalysisRequest request, ServerInformation information, BufferedWriter writer) throws IOException {
        AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getMediaId(), request.getPipelineId());
        EventLogger.getLogger().info("Sending command to server " + information.getServerId());
        //String command = System.getenv("KAFKA_SEND_COMMAND").replace("{topic}", information.getServerId());


//        BufferedWriter outputStreamWriter =
//                new BufferedWriter(new OutputStreamWriter(proc.getOutputStream()));
//
//        outputStreamWriter.write(commandString.toString());
//        outputStreamWriter.flush();
//        proc.destroy();

        writer.append("Analysis request sent\n").flush();
    }
}
