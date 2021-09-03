import clients.webclients.WebClient;
import clients.webclients.connection.Connection;
import com.google.gson.Gson;
import com.google.gson.JsonElement;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.serverinfo.codecs.ServerInformationDecoder;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Executor;
import java.util.concurrent.Executors;

/**
 * This class is essentially the glue that holds the entire Broker system together.
 * Instantiates listeners for new servers and clients, as well as creating observers
 * that pass messages between the components of the Broker system.
 */
public class Broker {
    private Thread serverListener;
    private Thread clientListener;
    private Thread serverParticipant;
    private final Object infoLock = new Object();
    private final Object topicLock = new Object();
    private final ArrayList<String> topics = new ArrayList<>();
    private final Executor clientConnections = Executors.newFixedThreadPool(8);
    private final ServerInformationHolder serverInformationHolder = new ServerInformationHolder();

    public Broker() {

        /*
        Observer class to handle new analysis servers. When a new analysis
        server is created, a new topic is made, so this class adds new topics
        when the listener discovers them.
         */
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

        /*
        This observer receives server information (such as IP address, usage, etc), deserializes it
        and adds it to a list of server information. A listener will poll for updated information
        and use this class to add the information to the list.
         */
        Observer<String> serverParticipantObserver = new Observer<String>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            /**
             * Decodes a JSON string into a ServerInformation object, and adds that object
             * to a list of server information objects.
             * @param message JSON string containing server information.
             */
            @Override
            public void onNext(@NonNull String message) {
                synchronized (infoLock) {

                    //Decodes the JSON message
                    ServerInformation information;
                    try {
                        JsonElement element = new Gson().fromJson(message, JsonElement.class);
                        information = new ServerInformationDecoder().deserialize(element, null,null);
                    } catch (Exception e) {
                        e.printStackTrace();
                        return;
                    }

                    //Extracts usage information from the message and calculates the usage of the server
                    Telemetry usageTelemetry = new TelemetryBuilder()
                            .setData(message)
                            .setCollector(TelemetryCollector.ALL)
                            .build();
                    long usage = usageTelemetry.getTelemetry();
                    information.setUsage(usage);
                    serverInformationHolder.add(information);
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };

        Observer<Socket> clientListenerObserver = new Observer<Socket>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            /**
             * Creates a new client connection.
             * @param connection Socket connection to the client.
             */
            @Override
            public void onNext(@NonNull Socket connection) {

                try {
                    Connection webConnection = ServiceLocator.getInstance()
                            .<Connection>createClass("ClientConnection", Socket.class)
                            .newInstance(connection);
                    WebClient client = ServiceLocator.getInstance()
                            .<WebClient>createClass("ClientParticipant", Connection.class, ServerInformationHolder.class)
                            .newInstance(webConnection, serverInformationHolder);
                    clientConnections.execute(client);
                } catch (InstantiationException | IllegalAccessException | InvocationTargetException e) {
                    e.printStackTrace();
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {

            }

            @Override
            public void onComplete() {

            }
        };

        try {
            serverListener = ServiceLocator
                    .getInstance().<Thread>createClass("ServerListener", Observer.class, List.class)
                    .newInstance(serverObservable, topics);
            serverListener.start();

            serverParticipant = ServiceLocator.getInstance()
                    .<Thread>createClass("ServerParticipantListener", Observer.class, List.class)
                    .newInstance(serverParticipantObserver, topics);
            serverParticipant.start();

            clientListener = ServiceLocator
                    .getInstance().<Thread>createClass("ClientListener", Observer.class)
                    .newInstance(clientListenerObserver);
            clientListener.start();
        } catch (InvocationTargetException | IllegalAccessException | InstantiationException e) {
            e.printStackTrace();
        }
    }
}
