package clients.webclients;

import clients.ClientBase;

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
