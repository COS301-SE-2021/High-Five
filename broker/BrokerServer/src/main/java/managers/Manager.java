package managers;

import dataclasses.serverinfo.ServerInformationHolder;
import managers.concurrencymanager.ConcurrencyManager;

import java.util.concurrent.*;

/**
 * Manager class to handle listener for new connections as well as
 * participants for all current connections.
 */
public abstract class Manager extends Thread {
    protected final ConcurrencyManager participants = ConcurrencyManager.getInstance();
    protected Thread listener;
    protected ServerInformationHolder serverInformationHolder;

    public Manager(ServerInformationHolder holder) {
        serverInformationHolder = holder;
    }
}
