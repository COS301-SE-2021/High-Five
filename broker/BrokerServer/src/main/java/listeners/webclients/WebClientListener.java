package listeners.webclients;

import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class WebClientListener extends ConnectionListener<Socket> {

    private boolean isConnected = false;
    private ServerSocket socketServer;

    public WebClientListener(Observer<Socket> notifier) throws IOException {
        super(notifier);
        socketServer = new ServerSocket(6666);
    }

    @Override
    protected void listen() throws IOException {
        while (true) {
            Socket connection = socketServer.accept();
            notify(connection);
        }
    }
}
