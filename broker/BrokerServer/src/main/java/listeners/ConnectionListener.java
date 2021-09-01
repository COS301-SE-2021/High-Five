package listeners;

import io.reactivex.rxjava3.core.Observer;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;

public abstract class ConnectionListener extends Thread {
    private final Observer<String> notifier;

    public ConnectionListener(Observer<String> notifier) {
        this.notifier = notifier;
    }

    protected abstract void listen() throws IOException, ParserConfigurationException, SAXException, InterruptedException;

    protected void notify(String message) {
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
