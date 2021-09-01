import clients.servers.KafkaServerParticipant;
import clients.servers.ServerParticipant;
import clients.webclients.ClientParticipant;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import dataclasses.websocket.WebSocketConnection;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import listeners.ConnectionListener;
import listeners.servers.KafkaMessageListener;
import listeners.webclients.WebClientListener;

import java.util.ArrayList;
import java.util.concurrent.Executor;
import java.util.concurrent.Executors;

public class Broker {
    private final ConnectionListener<String> serverListener;
    private ConnectionListener<WebSocketConnection> clientListener;
    private final ServerParticipant serverParticipant;
    private final Object infoLock = new Object();
    private final Object topicLock = new Object();
    private final ArrayList<String> topics = new ArrayList<>();
    private final Executor clientConnections = Executors.newFixedThreadPool(8);
    private final ServerInformationHolder serverInformationHolder = new ServerInformationHolder();

    public Broker() {
        Observer<String> serverObservable = new Observer<String>() {
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
        Observer<String> serverParticipantObserver = new Observer<String>() {
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

        Observer<WebSocketConnection> clientListenerObserver = new Observer<WebSocketConnection>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull WebSocketConnection connection) {
                clientConnections.execute(new ClientParticipant(serverInformationHolder));
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

        serverParticipant = new KafkaServerParticipant(serverParticipantObserver, topics);
        serverParticipant.start();

        clientListener = new WebClientListener(clientListenerObserver);
        clientListener.start();
    }
}
