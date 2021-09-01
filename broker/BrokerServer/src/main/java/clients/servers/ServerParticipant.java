package clients.servers;

import clients.ClientBase;
import dataclasses.websocket.Message;
import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.Observer;

public abstract class ServerParticipant extends Thread implements ClientBase {
    private final Observer<Message> serverInfoObservable;
    protected boolean beat = true;

    public ServerParticipant(Observer<Message> observable) {
        serverInfoObservable = observable;
    }

    public void notify(Message message) {
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
