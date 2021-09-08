package clients.webclients.strategy;

import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.serverinfo.ServerUsage;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import logger.EventLogger;

import java.io.*;
import java.util.Map;
import java.util.concurrent.ThreadLocalRandom;

public class StoredMediaAnalysisStrategy implements AnalysisStrategy{

    /**
     * Sends an instruction to an analysis server to process a file specified by the client and sends
     * the response back to the client.
     *
     * @param request Analysis request for media to be analysed
     * @param information Information about the server to perform analysis.
     * @param writer Writer to inform web client that media is going to be analysed
     */
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, BufferedWriter writer) throws IOException {

        //Uploaded media analysis doesn't require any hardware priority
        TelemetryBuilder builder = new TelemetryBuilder().setCollector(TelemetryCollector.ALL);
        ServerInformation info = information.get(builder);

        //Generate random topic to get information from server
        String responseTopic = "RESPONSE_TOPIC" + ThreadLocalRandom.current().nextInt();
        createResponseTopic(responseTopic);

        //Create new command
        AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getMediaId(), request.getPipelineId(), responseTopic);
        EventLogger.getLogger().info("Sending command to server " + info.getServerId());

        //Open process to send command
        String command = System.getenv("KAFKA_SEND_COMMAND").replace("{topic}", info.getServerId());
        ProcessBuilder processBuilder = new ProcessBuilder(command.split(" "));
        Process proc = processBuilder.start();

        //Send command to server
        BufferedWriter outputStreamWriter =
                new BufferedWriter(new OutputStreamWriter(proc.getOutputStream()));

        outputStreamWriter.write(commandString.toString());
        outputStreamWriter.flush();
        proc.destroy();

        //Get response from server
        String response = readResponse(responseTopic);

        //Send response to client
        writer.append(response).append("\n").flush();

        //Delete random topic
        deleteTopic(responseTopic);
    }

    /**
     * Creates a new topic for the server to use to send data back to the Broker
     * @param topic Random topic name
     */
    private void createResponseTopic(String topic) throws IOException {
        EventLogger.getLogger().info("Creating response topic " + topic);
        String command = System.getenv("KAFKA_CREATE_TOPIC").replace("{topic}", topic);
        ProcessBuilder builder = new ProcessBuilder(command.split(" "));
        Process proc = builder.start();
        try {
            proc.waitFor();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    /**
     * Fetch the response message from the server.
     * @param topic Random topic used for communication
     * @return Response from server
     */
    private String readResponse(String topic) throws IOException {
        EventLogger.getLogger().info("Reading response from topic " + topic);

        /*
        Create command to fetch message from topic.
        As this topic shall only exist for the server to send a response,
        it'll only ever have 1 message.
         */
        String exec = System.getenv("KAFKA_GET_MESSAGES")
                .replace("{topic}", topic)
                .replace("{offset}", "0"); //Since only one message is being sent.

        ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
        Process proc = builder.start();

        //Read response from server
        BufferedReader inputStreamReader =
                new BufferedReader(new InputStreamReader(proc.getInputStream()));

        String returnString = inputStreamReader.readLine();

        //send error response if string was null
        if (returnString == null) {
            //return error string
        }
        return returnString;
    }

    /**
     * Deletes the random topic once a response is read from it.
     *
     * @param topic Random topic to delete
     */
    private void deleteTopic(String topic) throws IOException {
        EventLogger.getLogger().info("Deleting random topic " + topic);
        String exec = System.getenv("KAFKA_DELETE_TOPIC").replace("{topic}", topic);
        ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
        try {
            builder.start().waitFor();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
