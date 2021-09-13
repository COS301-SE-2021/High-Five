package clients.webclients.strategy;

import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.analysiscommand.commandbody.StoredMediaCommandBody;
import dataclasses.clientrequest.requestbody.StoredMediaRequestBody;
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
     * @param handler Connection handler to send response to
     * @param connectionId Connection id
     */
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, ConnectionHandler handler, String connectionId) throws IOException {

        Properties props = new Properties();
        props.put("bootstrap.servers", "localhost:9092");
        Producer<String, String> producer = new KafkaProducer<>(props, new StringSerializer(), new StringSerializer());


        //Uploaded media analysis doesn't require any hardware priority
        TelemetryBuilder builder = new TelemetryBuilder().setCollector(TelemetryCollector.ALL);
        ServerInformation info = information.get(builder);

        //Create new command
        StoredMediaRequestBody body = (StoredMediaRequestBody) request.getBody();
        StoredMediaCommandBody commandBody = new StoredMediaCommandBody(body.getMediaId(), body.getPipelineId());
        AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getUserId(), commandBody);
        EventLogger.getLogger().info("Sending command to server " + info.getServerId());

        //Send command to server
        ProducerRecord<String, String> commandToSend = new ProducerRecord<>(info.getServerId(), 1, "Analyze", commandString.toString());
        producer.send(commandToSend);
        producer.close();


        //Get response from server
        String response = readResponse(info.getServerId());
        EventLogger.getLogger().info(response);

        //Send response to client
        ResponseObject responseObject = new ResponseObject(request.getRequestType(), null, response, connectionId);
        handler.onNext(responseObject);
    }

    /**
     * Fetch the response message from the server.
     * @param topic Random topic used for communication
     * @return Response from server
     */
    private String readResponse(String topic) throws IOException {
        EventLogger.getLogger().info("Reading response from topic " + topic);
        ConsumerRecord<String, String> message;
        String retMsg;

        //loop until we get a valid message
        while(true) {

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
                    consumer.close();
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

            message = messageList.get(0);
            retMsg = message.value();

            /*
            "heartbeat" may be contained in a valid message. A real heartbeat
            message will just have the word "heartbeat", so we just extract the first
            9 characters.
             */
            String possibleBeat = retMsg.substring(0, 9);

            /*
            Heartbeat messages mean that the AnalysisEngine is still analysing the
            media. While we are receiving "heartbeat", continue listening for a
            response.
             */
            if (!possibleBeat.contains("heartbeat")) {
                break;
            }
        }

        return retMsg;
    }
}
