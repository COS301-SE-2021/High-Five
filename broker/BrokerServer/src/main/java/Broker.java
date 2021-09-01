import clients.servers.ServerParticipant;
import dataclasses.websocket.Message;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.ObservableOnSubscribe;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import listeners.ConnectionListener;
import listeners.servers.KafkaMessageListener;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.concurrent.ThreadPoolExecutor;

public class Broker {
    private final ConnectionListener serverListener;
    private ConnectionListener clientListener;
    private ServerParticipant serverParticipant;
    private Object infoLock = new Object();
    private final LinkedList<Message> serverPerformanceInfo = new LinkedList<>();
    private final Object topicLock = new Object();
    private final ArrayList<String> topics = new ArrayList<>();
    private final Observer<Message> serverObservable;
    private final Observer<Message> serverParticipantObserver;

    public Broker() {
        serverObservable = new Observer<Message>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull Message message) {
                synchronized (topicLock) {
                    topics.add(message.getContent());
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };

        serverParticipantObserver = new Observer<Message>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull Message message) {

            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };

        serverListener = new KafkaMessageListener(serverObservable, topics);
        serverListener.start();


    }
}
