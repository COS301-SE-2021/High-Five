package managers.topicmanager;

import logger.EventLogger;

import java.io.IOException;
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
}
