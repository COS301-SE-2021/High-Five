package listeners.servers;

import com.google.gson.*;
import dataclasses.serverinfo.ServerRegistrationInfo;
import dataclasses.serverinfo.ServerTopics;
import dataclasses.serverinfo.codecs.ServerRegistrationInfoDecoder;
import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import logger.EventLogger;
import managers.topicmanager.TopicManager;
import org.apache.commons.io.IOUtil;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.common.TopicPartition;

import java.io.*;
import java.time.Duration;
import java.util.*;

/**
 * Listens for new topics in Kafka (which corresponds to a new analysis server joining the network).
 * When a new topic is discovered, it notifies the observer passed to this class by sending it the
 * new topic.
 */
public class KafkaTopicListener extends ConnectionListener<String> {

    private final ServerTopics topics;
    private long offset;

    public KafkaTopicListener(Observer<String> notifier, ServerTopics topics) {
        super(notifier);
        EventLogger.getLogger().info("Starting KafkaTopicListener");
        this.topics = topics;
        try {
            offset = loadServerList();
        } catch (IOException e) {
            offset = 0;
            EventLogger.getLogger().logException(e);
        }
    }

    /**
     * Listens for new topics from the Kafka broker. When a new topic is found,
     * the notifier is called, with the name of the topic passed.
     */
    @Override
    public void listen() throws IOException, InterruptedException {

        EventLogger.getLogger().info("Listening for new servers");

        while (true) {

            //Create consumer to listen for new topics
            Properties props = new Properties();
            props.setProperty("bootstrap.servers", "localhost:9092");
            props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
            props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
            KafkaConsumer<String, String> topicListener = new KafkaConsumer<>(props);

            TopicPartition registrationPartition = new TopicPartition("SERVER_REGISTRATION", 0);

            topicListener.assign(List.of(registrationPartition));
            topicListener.seek(registrationPartition, offset);

            List<ConsumerRecord<String, String>> registrationList = topicListener.
                    poll(Duration.ofSeconds(10)).records(registrationPartition);


            for (ConsumerRecord<String, String> registration : registrationList) {
                //Ensures that the topic does not already exist
                if (!topics.contains(registration.value()) && registration.value().length() > 0) {

                    //Deserialize the value
                    ServerRegistrationInfo registrationInfo;
                    try {
                        JsonElement element = new Gson().fromJson(registration.value(), JsonObject.class);
                        registrationInfo = new ServerRegistrationInfoDecoder().deserialize(element, null, null);
                    } catch (JsonParseException exception) {
                        //skip this value.
                        offset++;
                        continue;
                    }

                    //Check if the server has recently registered.
                    //We don't want to register old servers.
                    if ((System.currentTimeMillis()/1000L) - registrationInfo.getTimestamp() < 45) {
                        EventLogger.getLogger().info("New topic found: " + registrationInfo.getServerId());
                        addNewServer(++offset, registrationInfo);
                        TopicManager.getInstance().addTopic(registrationInfo.getServerId());
                        notify(registrationInfo.getServerId());
                    }
                }
            }

            topicListener.close();

            Thread.sleep(1000L);
        }
    }

    /**
     * Loads servers that have previously registered and returns the offset for the last message
     * in the server registration topic.
     * @return Last server offset
     */
    private long loadServerList() throws IOException {
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        InputStream is = classloader.getResourceAsStream("server_information.json");

        if (is == null) {
            EventLogger.getLogger().error("Failed to load server_information.json");
            return 0;
        }

        StringWriter writer = new StringWriter();
        IOUtil.copy(is, writer, "UTF-8");

        JsonElement serviceInfo = new Gson().fromJson(writer.toString(), JsonObject.class);

        long offset = serviceInfo.getAsJsonObject().get("offset").getAsLong();

        JsonArray servers = serviceInfo.getAsJsonObject().get("registered_servers").getAsJsonArray();

        for (JsonElement server : servers) {
            TopicManager.getInstance().addTopic(server.getAsString());
            notify(server.getAsString());
        }

        return offset;
    }

    /**
     * Add a new server to the list of servers.
     * @param offset new offset for file
     * @param server name of server
     * @throws IOException
     */
    private void addNewServer(long offset, ServerRegistrationInfo server) throws IOException {
        StringWriter writer = TopicManager.openResource("server_information.json");

        try {
            JsonObject serviceInfo = new Gson().fromJson(writer.toString(), JsonObject.class).getAsJsonObject();
            serviceInfo.add("offset", new JsonPrimitive(offset));
            serviceInfo.get("registered_servers").getAsJsonArray().add(server.getServerId());
            TopicManager.updateResource("server_information.json", serviceInfo);
        } catch (Exception e) {
            EventLogger.getLogger().logException(e);
        }
    }
}
