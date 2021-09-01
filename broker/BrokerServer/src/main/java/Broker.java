import dataclasses.websocket.Message;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.ObservableOnSubscribe;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import listeners.ConnectionListener;
import listeners.servers.KafkaMessageListener;

public class Broker {
    private ConnectionListener serverListener;
    private ConnectionListener clientListener;
    private Observer<Message> serverObservable;

    public Broker() {
        serverObservable = new Observer<Message>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull Message message) {
                System.out.println(message.getContent());
            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };
        serverListener = new KafkaMessageListener(serverObservable);
        serverListener.start();
    }
}
