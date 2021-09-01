package dataclasses.serverinfo;

import java.util.Comparator;
import java.util.LinkedList;
import java.util.concurrent.locks.ReentrantLock;
import java.util.stream.Collectors;

public class ServerInformationHolder {
    ReentrantLock lock = new ReentrantLock();
    private LinkedList<ServerInformation> serverPerformanceInfo = new LinkedList<>();

    public void add(ServerInformation info) {
        lock.lock();
        try {
            serverPerformanceInfo.removeIf((curInfo) -> curInfo.getServerId().equals(info.getServerId()));
            serverPerformanceInfo.add(info);
            serverPerformanceInfo = serverPerformanceInfo.stream()
                    .sorted(Comparator.comparing(ServerInformation::getUsage))
                    .collect(Collectors.toCollection(LinkedList::new));
        } finally {
            lock.unlock();
        }
    }

    public ServerInformation get() {
        lock.lock();
        try {
            ServerInformation information = serverPerformanceInfo.getFirst();
            serverPerformanceInfo.offer(serverPerformanceInfo.poll());
            return information;
        } finally {
            lock.unlock();
        }
    }
}
