package clients.servers;

import clients.ClientBase;
import io.reactivex.rxjava3.core.Observer;

public abstract class ServerParticipant extends Thread implements ClientBase {
    private final Observer<String> serverInfoObservable;
    protected boolean beat = true;

    public ServerParticipant(Observer<String> observable) {
        serverInfoObservable = observable;
    }

    public void notify(String message) {
        serverInfoObservable.onNext(message);
    }

    @Override
    public void run() {
        try {
            listen();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
