package dataclasses.serverinfo;

import org.apache.kafka.common.TopicPartition;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.locks.ReentrantLock;
import java.util.stream.Collectors;

public class ServerTopics {
    private final ArrayList<String> topics = new ArrayList<>();
    private final ReentrantLock lock = new ReentrantLock();


    public List<TopicPartition> getPartitions() {
        lock.lock();
        try {
            return topics.stream().map(e -> new TopicPartition(e, 0)).collect(Collectors.toList());
        } finally {
            lock.unlock();
        }
    }

    public void addTopic(String topic) {
        lock.lock();
        try {
            topics.add(topic);
        } finally {
            lock.unlock();
        }
    }

    public boolean contains(String topic) {
        return topics.contains(topic);
    }
}
