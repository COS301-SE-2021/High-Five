package listeners;

import io.reactivex.rxjava3.core.Observer;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;

public abstract class ConnectionListener<T> extends Thread {
    private final Observer<T> notifier;

    public ConnectionListener(Observer<T> notifier) {
        this.notifier = notifier;
    }

    protected abstract void listen() throws IOException, InterruptedException;

    protected void notify(T item) {
        notifier.onNext(item);
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
