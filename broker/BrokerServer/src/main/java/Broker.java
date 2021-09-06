import clients.webclients.WebClient;
import clients.webclients.connection.Connection;
import com.google.gson.*;
import dataclasses.serverinfo.*;
import dataclasses.serverinfo.codecs.ServerUsageDecoder;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.*;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import logger.EventLogger;
import org.apache.juli.logging.Log;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import servicelocator.wrappers.*;

import java.lang.reflect.*;
import java.net.Socket;
import java.util.*;
import java.util.concurrent.*;

/**
 * This class is essentially the glue that holds the entire Broker system together.
 * Instantiates listeners for new servers and clients, as well as creating observers
 * that pass messages between the components of the Broker system.
 */
public class Broker {
    private Thread serverListener;
    private Thread clientListener;
    private Thread serverParticipant;
    private final ArrayList<String> topics = new ArrayList<>();
    private final Executor clientConnections = Executors.newFixedThreadPool(8);
    private final ServerInformationHolder serverInformationHolder = new ServerInformationHolder();

    public Broker() {

        EventLogger.getLogger().info("Creating listener classes");

        /*
        Observer class to handle new analysis servers. When a new analysis
        server is created, a new topic is made, so this class adds new topics
        when the listener discovers them.
         */
        Observer<String> serverObservable = new Observer<>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            @Override
            public void onNext(@NonNull String message) {
                topics.add(message);
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
        Observer<String> serverParticipantObserver = new Observer<>() {
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

                //Decodes the JSON message
                ServerInformation information;
                try {
                    EventLogger.getLogger().info("Decoding server information from JSON message");
                    JsonElement element = new Gson().fromJson(message, JsonElement.class);
                    information = ServerInformationWrapper.get().deserialize(element, null, null);

                    //Extracts usage information from the message and calculates the usage of the server
                    ServerUsage usage = new ServerUsageDecoder().deserialize(element, null, null);


                    Telemetry usageTelemetry = new TelemetryBuilder()
                            .setData(usage)
                            .setCollector(TelemetryCollector.ALL)
                            .build();
                    information.setUsage(usageTelemetry.getTelemetry());
                    serverInformationHolder.add(information);
                } catch (Exception e) {
                    e.printStackTrace();
                    EventLogger.getLogger().error(e.getMessage());
                    return;
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {
                EventLogger.getLogger().error(e.getMessage());
            }

            @Override
            public void onComplete() {

            }
        };

        Observer<Socket> clientListenerObserver = new Observer<>() {
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
                    EventLogger.getLogger().info("Creating new client connection");
                    Connection webConnection = ClientConnectionWrapper.get(connection);
                    WebClient client = WebClientWrapper.get(webConnection, serverInformationHolder);
                    clientConnections.execute(client);
                } catch (InstantiationException | IllegalAccessException | InvocationTargetException e) {
                    EventLogger.getLogger().error(e.getMessage());
                    e.printStackTrace();
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {
                EventLogger.getLogger().error(e.getMessage());
            }

            @Override
            public void onComplete() {

            }
        };

        try {
            EventLogger.getLogger().info("Staring server listener");
            serverListener = ServerListenerWrapper.get(serverObservable, topics);
            serverListener.start();

            EventLogger.getLogger().info("Starting server participant listener");
            serverParticipant = ServerParticipantWrapper.get(serverParticipantObserver, topics);
            serverParticipant.start();

            EventLogger.getLogger().info("Starting client listener");
            clientListener = ClientListenerWrapper.get(clientListenerObserver);
            clientListener.start();
        } catch (InvocationTargetException | IllegalAccessException | InstantiationException e) {
            e.printStackTrace();
            EventLogger.getLogger().error(e.getMessage());
        }
    }
}
