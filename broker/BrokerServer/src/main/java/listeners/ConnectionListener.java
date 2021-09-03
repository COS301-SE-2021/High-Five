package listeners;

import io.reactivex.rxjava3.core.Observer;

import java.io.IOException;

/**
 * Abstract listener class that listeners extends.
 * @param <T> Data type for the observer to use.
 */
public abstract class ConnectionListener<T> extends Thread {
    private final Observer<T> notifier;

    public ConnectionListener(Observer<T> notifier) {
        this.notifier = notifier;
    }

    protected abstract void listen() throws IOException, InterruptedException;

    /**
     * Notifies the observer by passing it data.
     * @param item Data to pass to observer
     */
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
