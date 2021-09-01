import clients.servers.ServerParticipant;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import listeners.ConnectionListener;
import listeners.servers.KafkaMessageListener;

import java.util.ArrayList;

public class Broker {
    private final ConnectionListener serverListener;
    private ConnectionListener clientListener;
    private ServerParticipant serverParticipant;
    private final Object infoLock = new Object();
    private final Object topicLock = new Object();
    private final ArrayList<String> topics = new ArrayList<>();
    private final Observer<String> serverObservable;
    private final Observer<String> serverParticipantObserver;
    private final ServerInformationHolder serverInformationHolder = new ServerInformationHolder();

    public Broker() {
        serverObservable = new Observer<String>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull String message) {
                synchronized (topicLock) {
                    topics.add(message);
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };

        serverParticipantObserver = new Observer<String>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull String message) {
                synchronized (infoLock) {
                    String address = "stub"; //get address from message
                    String port = "stub"; //get port from message
                    String id = "stub"; //get id from message
                    String credentials = "stub"; //get credentials from message
                    Telemetry usageTelemetry = new TelemetryBuilder()
                            .setData(message)
                            .setCollector(TelemetryCollector.ALL)
                            .build();
                    long usage = usageTelemetry.getTelemetry();
                    serverInformationHolder.add(new ServerInformation(id, address, port, credentials, usage));
                }
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
