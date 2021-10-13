package managers.server;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import dataclasses.TopicAction.TopicAction;
import dataclasses.serverinfo.*;
import dataclasses.serverinfo.codecs.ServerUsageDecoder;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import logger.EventLogger;
import managers.Manager;
import servicelocator.wrappers.*;

import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;

public class ServerManagerImpl extends Manager {

    private final ServerTopics topics = new ServerTopics();

    public ServerManagerImpl(ServerInformationHolder holder) {
        super(holder);
    }

    public void run() {

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
                topics.addTopic(message);
            }

            @Override
            public void onError(@NonNull Throwable e) {
                EventLogger.getLogger().logException(e);
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
        Observer<TopicAction> serverParticipantObserver = new Observer<>() {
            @Override
            public void onSubscribe(@NonNull Disposable d) {

            }

            /**
             * Decodes a JSON string into a ServerInformation object, and adds that object
             * to a list of server information objects.
             * @param message JSON string containing server information.
             */
            @Override
            public void onNext(@NonNull TopicAction message) {

                //Decodes the JSON message
                ServerInformation information;
                try {
                    JsonElement element = new Gson().fromJson(message.message, JsonElement.class);
                    information = ServerInformationWrapper.get().deserialize(element, null, null);

                    if (message.action == TopicAction.Action.ADD_TOPIC) {

                        //Extracts usage information from the message and calculates the usage of the server
                        ServerUsage usage = new ServerUsageDecoder().deserialize(element, null, null);

                        serverInformationHolder.add(information, usage);
                    } else {
                        serverInformationHolder.remove(information);
                    }
                } catch (Exception e) {
                    EventLogger.getLogger().logException(e);
                }
            }

            @Override
            public void onError(@NonNull Throwable e) {
                EventLogger.getLogger().logException(e);
            }

            @Override
            public void onComplete() {

            }
        };

        try {
            EventLogger.getLogger().info("Staring server listener");
            listener = ServerListenerWrapper.get(serverObservable, topics);
            listener.start();

            EventLogger.getLogger().info("Starting server participant listener");
            Thread serverParticipant = ServerParticipantWrapper.get(serverParticipantObserver, topics);
            participants.execute(serverParticipant);
        } catch (InvocationTargetException | InstantiationException | IllegalAccessException e) {
            EventLogger.getLogger().logException(e);
        }
    }
}
