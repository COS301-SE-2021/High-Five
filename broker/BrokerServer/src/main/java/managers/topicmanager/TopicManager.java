package managers.topicmanager;

import com.google.gson.*;
import com.google.gson.reflect.TypeToken;
import logger.EventLogger;
import org.apache.commons.io.IOUtil;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.io.*;
import java.lang.reflect.Type;
import java.net.URL;
import java.net.URLDecoder;
import java.nio.charset.StandardCharsets;
import java.util.Map;
import java.util.concurrent.*;
import java.util.concurrent.locks.ReentrantLock;

public class TopicManager {
    private static TopicManager _instance;
    private final ReentrantLock lock = new ReentrantLock();
    private final ReentrantLock responseLock = new ReentrantLock();
    private final Executor actionExecutor = Executors.newFixedThreadPool(2);
    private final ReentrantLock topicLock = new ReentrantLock();
    private volatile boolean isLocked = false;
    private Map<String, String> responseIds;

    private TopicManager() {
        Gson gson = new GsonBuilder().create();
        try {
            StringWriter writer = openResource("response_messages.json");
            Type typeOfHashMap = new TypeToken<Map<String, String>>() { }.getType();
            if (writer == null) {
                return;
            }
            responseIds = gson.fromJson(writer.toString(), typeOfHashMap);
        } catch (IOException e) {
            EventLogger.getLogger().logException(e);
            System.exit(-1);
        }
    }

    public static TopicManager getInstance() {
        if (_instance == null) {
            _instance = new TopicManager();
        }
        return _instance;
    }

    /**
     * Delete a topic.
     * @param topic Name of topic to be deleted.
     */
    public void deleteTopic(String topic) {
        lock.lock();
        try {
            actionExecutor.execute(() -> {
                if (!isLocked) {
                    EventLogger.getLogger().warn("Not deleting topic as lock was not acquired");
                    return;
                }
                String exec = System.getenv("KAFKA_DELETE_TOPIC").replace("{topic}", topic);
                try {
                    ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
                    Process proc = builder.start();
                    deleteServer(topic);
                    proc.waitFor();
                } catch (IOException | InterruptedException e) {
                    EventLogger.getLogger().error(e.getMessage());
                } finally {
                    isLocked = false;
                }
            });
        }finally {
            lock.unlock();
        }
    }

    /**
     * Asynchronously creates a new topic for a new analysis server.
     *
     * @param topic New topic to register
     */
    public void addTopic(String topic) {
        lock.lock();
        try {
            actionExecutor.execute(() -> {
                if (!isLocked) {
                    EventLogger.getLogger().warn("Not adding topic as lock was not acquired");
                    return;
                }
                EventLogger.getLogger().info("Creating topic " + topic);
                ProcessBuilder builder = new ProcessBuilder(System.getenv("KAFKA_CREATE_TOPIC").
                        replace("{topic}", topic).split(" "));
                try {
                    Process proc = builder.start();
                    proc.waitFor();
                } catch (IOException | InterruptedException e) {
                    EventLogger.getLogger().logException(e);
                } finally {
                    isLocked = false;
                }
            });
        }finally {
            lock.unlock();
        }
    }

    private void deleteServer(String server) throws IOException {

        StringWriter writer = openResource("server_information.json");

        if (writer == null) {
            return;
        }


        try {
            JsonObject serviceInfo = new Gson().fromJson(writer.toString(), JsonObject.class).getAsJsonObject();

            JsonArray newArray = new JsonArray();

            for (JsonElement element: serviceInfo.get("registered_servers").getAsJsonArray()) {
                if (!element.getAsString().equals(server)) {
                    newArray.add(element);
                }
            }

            serviceInfo.add("registered_servers", newArray);

            updateResource("server_information.json", serviceInfo);
        } catch (Exception e) {
            EventLogger.getLogger().logException(e);
        }
    }

    /**
     * Checks if the message received is a new message from the server.
     * We do not want to send duplicate messages to the consumer
     * @param message Message to check
     * @return If the message is new or not
     */
    public boolean isNewMessage(ConsumerRecord<String, String> message) {
        responseLock.lock();
        try {
            //check if the message is in the map and if it's equal to the value in the map
            boolean isNewMessage = !responseIds.getOrDefault(message.topic(), "").equals(message.key());

            //update the map and write the map to a JSON file
            responseIds.put(message.topic(), message.key());
            Gson gson = new GsonBuilder().create();
            String json = gson.toJson(responseIds);
            JsonObject outputObject = new Gson().fromJson(json, JsonObject.class).getAsJsonObject();
            updateResource("response_messages.json", outputObject);
            return isNewMessage;
        } catch (IOException e) {
            EventLogger.getLogger().logException(e);
            return true;
        } finally {
            responseLock.unlock();
        }
    }

    /**
     * Updates a given resource.
     * @param resource the name of the resource
     * @param serviceInfo JSON object to write into the resource file
     * @throws FileNotFoundException
     */
    public static void updateResource(String resource, JsonObject serviceInfo) throws FileNotFoundException {
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        URL pathUrl = classloader.getResource(resource);
        if (pathUrl == null) {
            return;
        }
        String path = URLDecoder.decode(pathUrl.getPath(), StandardCharsets.UTF_8);
        PrintWriter jsonWriter = new PrintWriter(path);
        jsonWriter.append(serviceInfo.toString()).flush();
    }

    /**
     * Returns a StringWriter to read the given resource
     * @param resourceName Resource to read.
     * @return StringWriter
     * @throws IOException
     */
    public static StringWriter openResource(String resourceName) throws IOException {
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        InputStream is = classloader.getResourceAsStream(resourceName);

        if (is == null) {
            EventLogger.getLogger().error("Failed to load " + resourceName);
            return null;
        }

        StringWriter writer = new StringWriter();
        IOUtil.copy(is, writer, "UTF-8");
        return writer;
    }

    public void lockTopic() {
        topicLock.lock();
        isLocked = true;
    }

    public void unlockTopic() {

        while(isLocked) {
            try {
                Thread.sleep(200);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }
        topicLock.unlock();
    }
}
