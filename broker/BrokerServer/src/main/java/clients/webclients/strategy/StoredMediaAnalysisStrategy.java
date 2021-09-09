package clients.webclients.strategy;

import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import logger.EventLogger;
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

        try {
            Thread.sleep(1000L);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }

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

        //Initialise the Kafka consumer to read a response from the communication partition of the topic
        Properties props = new Properties();
        props.setProperty("bootstrap.servers", "localhost:9092");
        props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
        props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
        KafkaConsumer<String, String> consumer = new KafkaConsumer<>(props);
        TopicPartition partition = new TopicPartition(topic, 1);

        consumer.assign(List.of(partition));
        consumer.seekToEnd(List.of(partition));

        List<ConsumerRecord<String, String>> messageList = consumer.poll(Duration.ofSeconds(30)).records(partition);

        if (messageList.size() == 0) {
            throw new IOException("No messages could be read from topic: " + partition.topic());
        }

        ConsumerRecord<String, String> message = messageList.get(0);

        EventLogger.getLogger().info(message.toString());

        consumer.close();

        return message.value();
    }
}
