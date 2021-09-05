package listeners.webclients;

import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import logger.EventLogger;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * This class listens for incoming requests. When a new request is made,
 * this class notifies the observer passed to it, by passing in the connection to
 * the request.
 */
public class WebClientListener extends ConnectionListener<Socket> {

    private boolean isConnected = false;
    private ServerSocket socketServer;

    /**
     * Instantiates the server socket to listen for new requests
     * @param notifier Observer to be notified
     */
    public WebClientListener(Observer<Socket> notifier) throws IOException {
        super(notifier);
        EventLogger.getLogger().info("Starting WebClientListener");

        int port = Integer.parseInt(System.getenv("BROKER_CLIENT_PORT"));

        socketServer = new ServerSocket(port);
    }

    /**
     * Listens for new connections and notifies the observer once the connection
     * has been established.
     */
    @Override
    protected void listen() throws IOException {
        EventLogger.getLogger().info("Listening for new clients");
        while (true) {
            Socket connection = socketServer.accept();
            EventLogger.getLogger().info("New client connected");
            notify(connection);
        }
    }
}
