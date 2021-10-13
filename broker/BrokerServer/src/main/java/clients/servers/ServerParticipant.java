package clients.servers;

import clients.ClientBase;
import dataclasses.TopicAction.TopicAction;
import io.reactivex.rxjava3.core.Observer;

public abstract class ServerParticipant extends Thread implements ClientBase {
    private final Observer<TopicAction> serverInfoObservable;
    protected boolean beat = true;

    public ServerParticipant(Observer<TopicAction> observable) {
        serverInfoObservable = observable;
    }

    public void notify(TopicAction message) {
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
