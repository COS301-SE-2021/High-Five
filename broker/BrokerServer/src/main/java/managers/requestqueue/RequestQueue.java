package managers.requestqueue;

import clients.webclients.strategy.*;
import dataclasses.clientrequest.RequestQueueItem;
import logger.EventLogger;
import managers.concurrencymanager.ConcurrencyManager;

import java.io.IOException;
import java.util.LinkedList;
import java.util.concurrent.locks.ReentrantLock;

/**
 * This class is used to queue analysis requests and dispatch them to analysis engines.
 * This class allows multiple analysis requests to be handled, from one or multiple end
 * users.
 */
public class RequestQueue {
    private final ReentrantLock requestLock = new ReentrantLock();
    private final LinkedList<RequestQueueItem> analysisRequests = new LinkedList<>();
    private static RequestQueue _instance;
    private static final int MAX_RETRIES = 1800;

    /**
     * Private constructor for the RequestQueue class. This constructor creates a thread
     * which waits for analysis requests and dispatches these requests to eligible analysis engines.
     */
    private RequestQueue() {
        ConcurrencyManager.getInstance().execute(() -> {
            while (true) {
                RequestQueueItem request = nextRequest();

                //Check if there is a request and an available analysis engine to handle the request
                if (request != null && !request.informationHolder.isEmpty()) {
                    ConcurrencyManager.getInstance().execute(() -> {
                        try {
                            //Process request based on analysis type
                            if (request.request.getRequestType().contains("Analyze")) {
                                EventLogger.getLogger().info("Performing analysis on uploaded media");
                                new StoredMediaAnalysisStrategy().processRequest(request.request, request.informationHolder, request.handler, request.connection.getConnectionId());
                            } else if (request.request.getRequestType().contains("StartLiveAnalysis")){
                                EventLogger.getLogger().info("Performing live analysis request");
                                new LiveAnalysisStrategy().processRequest(request.request, request.informationHolder, request.handler, request.connection.getConnectionId());
                            } else {
                                EventLogger.getLogger().info("Performing live stream request");
                                new LiveStreamStrategy().processRequest(request.request, request.informationHolder, request.handler, request.connection.getConnectionId());
                            }
                        } catch (IOException e) {
                            //check if the request has expired and add back to list if not expired
                            if (request.expires > System.currentTimeMillis()) {
                                _addToQueue(request);
                            } else {
                                EventLogger.getLogger().logException(e);
                            }
                        }
                    });

                } else if (request != null && request.expires > System.currentTimeMillis()){
                    _addToQueue(request);
                }
                try {
                    Thread.sleep(1000L);
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
            }
        });
    }

    /**
     * Creates a new instance of the singleton if not created yet
     * and returns that instance.
     * @return instance of RequestQueue
     */
    public static RequestQueue getInstance() {
        if (_instance == null) {
            _instance = new RequestQueue();
        }
        return _instance;
    }

    /**
     * Adds an analysis request to the queue
     * @param request Analysis request to be added to the queue
     */
    public void addToQueue(RequestQueueItem request) {
        EventLogger.getLogger().info("Adding request to request queue");
        _addToQueue(request);
    }

    /**
     * Implementation of the addToQueue function.
     * @param request
     */
    private void _addToQueue(RequestQueueItem request) {
        requestLock.lock();
        try {
            analysisRequests.add(request);
        } finally {
            requestLock.unlock();
        }
    }

    /**
     * Returns the next analysis request or null if there are no requests.
     * @return analysis request or null
     */
    private RequestQueueItem nextRequest() {
        requestLock.lock();
        try {
            if (analysisRequests.size() > 0) {
                return analysisRequests.removeFirst();
            } else {
                return null;
            }
        } finally {
            requestLock.unlock();
        }
    }
}
