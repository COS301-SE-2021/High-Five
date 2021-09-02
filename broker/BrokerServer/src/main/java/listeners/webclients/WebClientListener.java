package listeners.webclients;

import dataclasses.sockets.WebSocketConnection;
import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
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
    protected void listen() throws IOException, ParserConfigurationException, SAXException, InterruptedException {
        while (true) {
            Socket connection = socketServer.accept();
            notify(connection);
        }
    }
}
