package clients.servers;

import dataclasses.serverinfo.ServerTopics;
import io.reactivex.rxjava3.core.Observer;
import logger.EventLogger;
import managers.topicmanager.TopicManager;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.common.TopicPartition;

import java.io.*;
import java.time.Duration;
import java.util.List;
import java.util.Properties;
import java.util.regex.*;

public class KafkaServerParticipant extends ServerParticipant {

    private final ServerTopics topics;

    public KafkaServerParticipant(Observer<String> observable, ServerTopics topics) {
        super(observable);
        EventLogger.getLogger().info("Creating new KafkaServerParticipant");
        this.topics = topics;
    }

    @Override
    public boolean heartbeat() {
        return beat;
    }

    /**
     * Iterate through the list of given topics and read the messages from each topic.
     * Only the last message sent will be used to notify the given observer.
     */
    @Override
    public void listen() throws InterruptedException {
        EventLogger.getLogger().info("Listening for new messages from servers");
        while (true) {

            //Create consumer to listen to new performance messages from servers
            List<TopicPartition> partitions = topics.getPartitions();
            Properties props = new Properties();
            props.setProperty("bootstrap.servers", "localhost:9092");
            props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
            props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
            KafkaConsumer<String, String> consumer = new KafkaConsumer<>(props);

            consumer.assign(partitions);

            //Iterate through each server
            for (TopicPartition partition : partitions) {

                //Get the last message from the server
                long offset = consumer.position(partition);
                if (offset == 0) {
                    continue;
                }
                consumer.seek(partition, offset-1);

                List<ConsumerRecord<String, String>> messageList = consumer.poll(Duration.ofSeconds(10)).records(partition);

                //Check if the server has any messages
                if (messageList.size() == 0) {
                    //EventLogger.getLogger().info("No messages found!");
                    continue;
                }

                ConsumerRecord<String, String> msg = messageList.get(0);

                //delete topic if the last message sent is older than 45 seconds. This means the server is
                //offline.
                if ((System.currentTimeMillis() / 1000L) - getMessageTime(msg.value()) > 45) {
                    EventLogger.getLogger().info("Deleting topic: " + msg.topic());
                    TopicManager.getInstance().deleteTopic(msg.topic());
                    topics.deleteTopic(msg.topic());
                } else {
                    notify(msg.value());
                }
            }

            consumer.close();
            Thread.sleep(1000L);
        }
    }

    /**
     * Retrieve the time the message was sent.
     * @param message The message to fetch the time from.
     * @return UNIX timestamp of time message was sent.
     */
    private long getMessageTime(String message) {
        Matcher timePattern = Pattern.compile("\"timestamp\": *(\\d+).*[,}]").matcher(message);
        if (!timePattern.find()) {
            return 0;
        }
        return Long.parseLong(timePattern.group(1));
    }
}
