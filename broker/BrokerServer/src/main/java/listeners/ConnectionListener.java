package listeners;

import dataclasses.websocket.Message;
import io.reactivex.rxjava3.core.Observer;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;

public abstract class ConnectionListener extends Thread {
    private final Observer<Message> notifier;

    public ConnectionListener(Observer<Message> notifier) {
        this.notifier = notifier;
    }

    protected abstract void listen() throws IOException, ParserConfigurationException, SAXException, InterruptedException;

    protected void notify(Message message) {
        notifier.onNext(message);
    }

    @Override
    public void run() {
        try {
            listen();
        } catch (Exception e) {
            notifier.onError(e);
        }
    }
}
