package listeners.webclients;

import dataclasses.websocket.WebSocketConnection;
import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;

public class WebClientListener extends ConnectionListener<WebSocketConnection> {

    private boolean isConnected = false;

    public WebClientListener(Observer<WebSocketConnection> notifier) {
        super(notifier);
    }

    @Override
    protected void notify(WebSocketConnection item) {
        super.notify(item);
        isConnected = true;
    }

    @Override
    protected void listen() throws IOException, ParserConfigurationException, SAXException, InterruptedException {
//        while (true) {
//            WebSocketConnection connection = new WebSocketConnection();
//            connection.doOnOpen(this::notify);
//            while (!isConnected) {
//                Thread.sleep(1000L);
//            }
//            isConnected = false;
//        }
    }
}
