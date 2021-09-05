package clients.webclients.strategy;

import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;

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
    public void processRequest(AnalysisRequest request, ServerInformation information, Writer writer) throws IOException {
        Runtime rt = Runtime.getRuntime();
        AnalysisCommand commandString = new AnalysisCommand(request.getMediaType(), request.getMediaID());
        String command = System.getenv("KAFKA_SEND_COMMAND").replace("{topic}", information.getServerId());
        Process proc = rt.exec(command);


        BufferedWriter outputStreamWriter =
                new BufferedWriter(new OutputStreamWriter(proc.getOutputStream()));

        outputStreamWriter.write(commandString.toString());
        outputStreamWriter.flush();
        proc.destroy();

        writer.write("Analysis request sent");
        writer.flush();
    }
}
