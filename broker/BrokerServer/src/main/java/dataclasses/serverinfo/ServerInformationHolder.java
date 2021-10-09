package dataclasses.serverinfo;

import dataclasses.telemetry.builder.TelemetryBuilder;

import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.locks.ReentrantLock;
import java.util.stream.Collectors;

/**
 * Holds a list of ServerInformation class. This class also ensures that only one thread at a time may
 * access this list by using a lock.
 */
public class ServerInformationHolder {
    ReentrantLock lock = new ReentrantLock();
    private final HashMap<ServerInformation, ServerUsage> serverPerformanceInfo = new HashMap<>();

    /**
     * Adds a ServerInformation object to the list. Once the item is added, the list is
     * sorted by the usage of the server.
     *
     * @param info ServerInformation object to add
     */
    public void add(ServerInformation info, ServerUsage usage) {
        lock.lock();
        try {
            for (var entries : serverPerformanceInfo.entrySet()) {
                if (entries.getKey().getServerId().equals(info.getServerId())) {
                    entries.getValue().cpu = usage.cpu;
                    entries.getValue().gpu = usage.gpu;
                    entries.getValue().net = usage.net;
                    entries.getValue().disk = usage.disk;
                    return;
                }
            }
            serverPerformanceInfo.put(info, usage);
        } finally {
            lock.unlock();
        }
    }

    public void remove(ServerInformation info) {
        lock.lock();
        try {
            serverPerformanceInfo.entrySet().removeIf(item -> item.getKey().getServerId().equals(info.getServerId()));
        } finally {
            lock.unlock();
        }
    }

    public boolean isEmpty() {
        lock.lock();
        try {
            return serverPerformanceInfo.isEmpty();
        } finally {
            lock.unlock();
        }
    }

    /**
     * Fetches the server information with the least usage as determined by the TelemetryBuilder
     * passed in.
     *
     * @param performanceSelector Builder to determine usage of server.
     * @return First ServerInformation object from the list, or null if the list is empty.
     */
    public ServerInformation get(TelemetryBuilder performanceSelector) {
        lock.lock();
        try {
            if (serverPerformanceInfo.size() == 0) {
                return null;
            }

            //Make copy of map (as to not alter values in the original list)
            Map<ServerInformation, ServerUsage> tmp = serverPerformanceInfo.entrySet().stream()
                    .collect(Collectors.toMap(e -> e.getKey().clone(), e -> e.getValue().clone()));

            //Calculating the usage information of the items in the temporary map
            for (var item : tmp.entrySet()) {
                long usage = performanceSelector.setData(item.getValue()).build().getTelemetry();
                item.getKey().setUsage(usage);
            }

            //Sort the keys in the map (the one with the least usage should be first)
            List<ServerInformation> list = tmp.keySet()
                    .stream().sorted(Comparator.comparing(ServerInformation::getUsage)).collect(Collectors.toList());

            return list.get(0); // Return first item
        } finally {
            lock.unlock();
        }
    }
}
