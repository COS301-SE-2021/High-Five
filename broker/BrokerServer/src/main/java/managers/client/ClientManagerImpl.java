package managers.client;

import clients.webclients.WebClient;
import clients.webclients.connection.Connection;
import dataclasses.serverinfo.ServerInformationHolder;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import logger.EventLogger;
import managers.Manager;
import servicelocator.wrappers.ClientConnectionWrapper;
import servicelocator.wrappers.ClientListenerWrapper;
import servicelocator.wrappers.WebClientWrapper;

import java.lang.reflect.InvocationTargetException;
import java.net.Socket;

/**
 * This class manages a ClientListener, which listens out for new connections,
 * and adds these new connections to a pool of participants.
 */
public class ClientManagerImpl extends Manager {
    public ClientManagerImpl(ServerInformationHolder holder) {
        super(holder, 8);
    }

    public void run() {
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
                    participants.execute(client);
                } catch (InstantiationException | IllegalAccessException | InvocationTargetException e) {
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
            listener = ClientListenerWrapper.get(clientListenerObserver);
            EventLogger.getLogger().info("Starting client listener");
            listener.start();
        } catch (InvocationTargetException | InstantiationException | IllegalAccessException e) {
            EventLogger.getLogger().logException(e);
        }
    }
}
