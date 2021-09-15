package clients.webclients.strategy;

import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import logger.EventLogger;
import managers.topicmanager.TopicManager;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.clients.producer.*;
import org.apache.kafka.common.TopicPartition;
import org.apache.kafka.common.serialization.StringSerializer;

import java.io.*;
import java.time.Duration;
import java.util.List;
import java.util.Properties;

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

        Properties props = new Properties();
        props.put("bootstrap.servers", "localhost:9092");
        Producer<String, String> producer = new KafkaProducer<>(props, new StringSerializer(), new StringSerializer());


        //Uploaded media analysis doesn't require any hardware priority
        TelemetryBuilder builder = new TelemetryBuilder().setCollector(TelemetryCollector.ALL);
        ServerInformation info = information.get(builder);

        //Create new command
        AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getMediaId(), request.getPipelineId());
        EventLogger.getLogger().info("Sending command to server " + info.getServerId());

        //Send command to server
        ProducerRecord<String, String> commandToSend = new ProducerRecord<>(info.getServerId(), 1, "Analyze", commandString.toString());
        producer.send(commandToSend);
        producer.close();


        //Get response from server
        String response = readResponse(info.getServerId());

        //Send response to client
        writer.append(response).append("\n").flush();
    }

    /**
     * Fetch the response message from the server.
     * @param topic Random topic used for communication
     * @return Response from server
     */
    private String readResponse(String topic) throws IOException {
        EventLogger.getLogger().info("Reading response from topic " + topic);

        List<ConsumerRecord<String, String>> messageList = null;

        boolean messageFound = false;

        //Listen for a new message from the server.
        for (int i = 0; i < 10; i++) {
            //Initialise the Kafka consumer to read a response from the communication partition of the topic
            Properties props = new Properties();
            props.setProperty("bootstrap.servers", "localhost:9092");
            props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
            props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");

            TopicPartition partition = new TopicPartition(topic, 2);

            KafkaConsumer<String, String> consumer = new KafkaConsumer<>(props);
            consumer.assign(List.of(partition));
            consumer.seekToEnd(List.of(partition));

            long position = consumer.position(partition);
            System.out.println(position);
            if (position > 0) {
                position--;
            }
            consumer.seek(partition, position);

             messageList = consumer.poll(Duration.ofSeconds(5)).records(partition);

             if (messageList.size() > 0) {
                 messageFound = TopicManager.getInstance().isNewMessage(messageList.get(0));
             }

             //Only go through if we have a message that has not been sent before
             if (messageFound) {
                 break;
             }

            consumer.close();

            try {
                Thread.sleep(1000L);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }

        /*
        Throw an exception if the server has not responded.
        A server is non-responsive when it has sent no new messages, or no messages at all.
         */
        if (messageList.size() == 0 || !messageFound) {
            throw new IOException("No new messages could be read from topic: " + topic);
        }

        ConsumerRecord<String, String> message = messageList.get(0);

        return message.value();
    }
}
