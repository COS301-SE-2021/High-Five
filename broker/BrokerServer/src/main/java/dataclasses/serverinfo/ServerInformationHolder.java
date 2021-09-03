package dataclasses.serverinfo;

import java.util.Comparator;
import java.util.LinkedList;
import java.util.concurrent.locks.ReentrantLock;
import java.util.stream.Collectors;

/**
 * Holds a list of ServerInformation class. This class also ensures that only one thread at a time may
 * access this list by using a lock.
 */
public class ServerInformationHolder {
    ReentrantLock lock = new ReentrantLock();
    private LinkedList<ServerInformation> serverPerformanceInfo = new LinkedList<>();

    /**
     * Adds a ServerInformation object to the list. Once the item is added, the list is
     * sorted by the usage of the server.
     *
     * @param info ServerInformation object to add
     */
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

    /**
     * Fetches the first element from the server information list.
     * Once this element is fetched, it is moved to the back of the list, as it's usage will soon
     * be altered.
     *
     * @return First ServerInformation object from the list, or null if the list is empty.
     */
    public ServerInformation get() {
        lock.lock();
        try {
            if (serverPerformanceInfo.size() > 0) {
                ServerInformation information = serverPerformanceInfo.getFirst();
                serverPerformanceInfo.offer(serverPerformanceInfo.poll());
                return information;
            } else {
                return null;
            }
        } finally {
            lock.unlock();
        }
    }
}
