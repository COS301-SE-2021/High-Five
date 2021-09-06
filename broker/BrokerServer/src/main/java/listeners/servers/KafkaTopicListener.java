package listeners.servers;

import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import logger.EventLogger;

import java.io.*;
import java.util.*;

/**
 * Listens for new topics in Kafka (which corresponds to a new analysis server joining the network).
 * When a new topic is discovered, it notifies the observer passed to this class by sending it the
 * new topic.
 */
public class KafkaTopicListener extends ConnectionListener<String> {

    private final List<String> topics;

    public KafkaTopicListener(Observer<String> notifier, List<String> topics) {
        super(notifier);
        EventLogger.getLogger().info("Starting KafkaTopicListener");
        this.topics = topics;
    }

    /**
     * Listens for new topics from the Kafka broker. When a new topic is found,
     * the notifier is called, with the name of the topic passed.
     */
    @Override
    public void listen() throws IOException, InterruptedException {

        EventLogger.getLogger().info("Listening for new servers");

        while (true) {
            //Runtime rt = Runtime.getRuntime();
            ProcessBuilder builder = new ProcessBuilder(System.getenv("KAFKA_LIST_TOPICS").split(" "));
            Process proc = builder.start();


            String line;

            BufferedReader inputStreamReader =
                    new BufferedReader(new InputStreamReader(proc.getInputStream()));
            while ((line = inputStreamReader.readLine()) != null) {

                //Ensures that the topic does not already exist
                if (!isIgnored(line) && !topics.contains(line) && line.length() > 0) {
                    EventLogger.getLogger().info("New topic found: " + line);
                    notify(line);
                }
            }

            BufferedReader errorStreamReader =
                    new BufferedReader(new InputStreamReader(proc.getErrorStream()));
            while ((line = errorStreamReader.readLine()) != null) {
                EventLogger.getLogger().warn(line);
            }
            Thread.sleep(1000L);
        }
    }

    /**
     * Checks if the given topic is to be ignored. Primarily used for system reserved
     * topics we don't want the Broker to listen to.
     * @param topic Topic to check
     * @return Whether the topic should be ignored or not.
     */
    private boolean isIgnored(String topic) {

        //open and read file containing ignored topics
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        InputStream is = classloader.getResourceAsStream("ignored_topics.txt");

        if (is == null) {
            EventLogger.getLogger().warn("Cannot read ignored_topics.txt");
            return false;
        }

        BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(is));
        ArrayList<String> ignoredTopics = new ArrayList<>();

        //Add these topics to a list
        String line;
        while (true) {
            try {
                if ((line = bufferedReader.readLine()) == null) break;
            } catch (IOException e) {
                EventLogger.getLogger().error(e.getMessage());
                return false;
            }
            ignoredTopics.add(line);
        }
        //Check if the list contains the topic
        return ignoredTopics.stream().anyMatch(topic::contains);
    }
}
