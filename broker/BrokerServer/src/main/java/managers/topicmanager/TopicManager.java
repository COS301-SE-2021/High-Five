package managers.topicmanager;

import com.google.gson.*;
import logger.EventLogger;
import org.apache.commons.io.IOUtil;

import java.io.*;
import java.net.URL;
import java.net.URLDecoder;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.*;
import java.util.concurrent.locks.ReentrantLock;

public class TopicManager {
    private static TopicManager _instance;
    private final ReentrantLock lock = new ReentrantLock();
    private final Executor actionExecutor = Executors.newFixedThreadPool(2);

    private TopicManager() {

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
                String exec = System.getenv("KAFKA_DELETE_TOPIC").replace("{topic}", topic);
                try {
                    ProcessBuilder builder = new ProcessBuilder(exec.split(" "));
                    builder.start();
                    deleteServer(topic);
                } catch (IOException e) {
                    EventLogger.getLogger().error(e.getMessage());
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
                ProcessBuilder builder = new ProcessBuilder(System.getenv("KAFKA_CREATE_TOPIC").
                        replace("{topic}", topic).split(" "));
                try {
                    Process proc = builder.start();
                    proc.waitFor();
                } catch (IOException | InterruptedException e) {
                    EventLogger.getLogger().logException(e);
                }
            });
        }finally {
            lock.unlock();
        }
    }

    private void deleteServer(String server) throws IOException {
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        InputStream is = classloader.getResourceAsStream("server_information.json");

        if (is == null) {
            EventLogger.getLogger().error("Failed to load server_information.json");
            return;
        }

        StringWriter writer = new StringWriter();
        IOUtil.copy(is, writer, "UTF-8");

        try {
            JsonObject serviceInfo = new Gson().fromJson(writer.toString(), JsonObject.class).getAsJsonObject();

            JsonArray newArray = new JsonArray();

            for (JsonElement element: serviceInfo.get("registered_servers").getAsJsonArray()) {
                if (!element.getAsString().equals(server)) {
                    newArray.add(element);
                }
            }

            serviceInfo.add("registered_servers", newArray);

            updateServerInformation(classloader, serviceInfo);
        } catch (Exception e) {
            EventLogger.getLogger().logException(e);
        }
    }

    public static void updateServerInformation(ClassLoader classloader, JsonObject serviceInfo) throws FileNotFoundException {
        URL pathUrl = classloader.getResource("server_information.json");
        if (pathUrl == null) {
            return;
        }
        String path = URLDecoder.decode(pathUrl.getPath(), StandardCharsets.UTF_8);
        PrintWriter jsonWriter = new PrintWriter(path);
        jsonWriter.append(serviceInfo.toString()).flush();
        return;
    }
}
