package clients.servers;

import io.reactivex.rxjava3.core.Observer;
import logger.EventLogger;

import java.io.*;
import java.util.List;
import java.util.regex.*;

public class KafkaServerParticipant extends ServerParticipant {

    private final List<String> topics;

    public KafkaServerParticipant(Observer<String> observable, List<String> topics) {
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

            //Enhanced for loop not used because of concurrent access to topics
            for (int i = 0; i < topics.size(); i++) {

                String topic = topics.get(i);

                int offset = 0;

                try {
                    offset = getLastOffset(topic);
                } catch (IOException e) {
                    EventLogger.getLogger().error(e.getMessage());
                    continue;
                }

                //No messages were found
                if (offset < 0 ) {
                    EventLogger.getLogger().info("No messages found!");
                    continue;
                }

                Process proc;

                String exec = System.getenv("KAFKA_GET_MESSAGES")
                        .replace("{topic}", topic)
                        .replace("{offset}", Integer.toString(offset));
                try {
                    ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
                    proc = builder.start();
                } catch (IOException e) {
                    EventLogger.getLogger().error(e.getMessage());
                    beat = false;
                    return;
                }

                String line;
                String msg = null;

                //Read all the messages from the topic, but only use the latest message.
                BufferedReader inputStreamReader =
                        new BufferedReader(new InputStreamReader(proc.getInputStream()));
                while (true) {
                    try {
                        if ((line = inputStreamReader.readLine()) == null) break;
                    } catch (IOException e) {
                        beat = false;
                        EventLogger.getLogger().error(e.getMessage());
                        return;
                    }
                    msg = line;
                }

                if (msg == null) {
                    continue;
                }

                //delete topic if the last message sent is older than 45 seconds. This means the server is
                //offline.
                if (((int) System.currentTimeMillis()/1000L) - getMessageTime(msg) > 45 ) {
                    EventLogger.getLogger().info("Deleting topic: " + topic);
                    deleteTopic(topic);
                } else {
                    notify(msg);
                    proc.destroy();
                }
            }
            Thread.sleep(1000L);
        }
    }

    /**
     * Delete a topic.
     * @param topic Name of topic to be deleted.
     */
    private void deleteTopic(String topic) {
        String exec = System.getenv("KAFKA_DELETE_TOPIC").replace("{topic}", topic);
        try {
            ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
            builder.start();
        } catch (IOException e) {
            EventLogger.getLogger().error(e.getMessage());
        }
    }

    /**
     * Retrieve the time the message was sent.
     * @param message The message to fetch the time from.
     * @return UNIX timestamp of time message was sent.
     */
    private long getMessageTime(String message) {
        Matcher timePattern = Pattern.compile("timestamp: *(\\d+).*[,}]").matcher(message);
        if (!timePattern.find()) {
            return 0;
        }
        return Long.parseLong(timePattern.group(1));
    }

    /**
     * Fetches the last offset from a Kafka topic. This is used to fetch the latest
     * message from the topic.
     * @param topic Kafka topic to get offset from
     * @return Latest offset
     */
    private int getLastOffset(String topic) throws IOException {
        ProcessBuilder builder = new ProcessBuilder(System.getenv("KAFKA_GET_OFFSET")
                .replace("{topic}", topic).split(" "));

        Process process= builder.start();

        BufferedReader inputStreamReader =
                new BufferedReader(new InputStreamReader(process.getInputStream()));
        String line = inputStreamReader.readLine();
        process.destroy();

        Matcher matcher = Pattern.compile(topic + ":0:(\\d+)").matcher(line);

        if (!matcher.find()) {
            return -1;
        } else {
            return Integer.parseInt(matcher.group(1)) - 1;
        }
    }
}
