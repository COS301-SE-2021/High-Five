package clients.webclients;

import clients.ClientBase;

/**
 * Abstract class for WebClients. Extends the Runnable interface and automatically
 * calls the listen() method, which derived classes need to implement.
 */
public abstract class WebClient implements ClientBase, Runnable {

    @Override
    public void run() {
        try {
            listen();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
