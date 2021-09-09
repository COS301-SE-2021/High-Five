package managers;

import dataclasses.serverinfo.ServerInformationHolder;

import java.util.concurrent.*;

/**
 * Manager class to handle listener for new connections as well as
 * participants for all current connections.
 */
public abstract class Manager extends Thread {
    protected final Executor participants;
    protected Thread listener;
    protected ServerInformationHolder serverInformationHolder;

    public Manager(ServerInformationHolder holder, int numThreads) {
        serverInformationHolder = holder;
        participants = Executors.newFixedThreadPool(numThreads);
    }
}
