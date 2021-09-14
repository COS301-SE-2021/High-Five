package clients.webclients.connectionhandler;

import clients.webclients.connection.Connection;
import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observer;
import io.reactivex.rxjava3.disposables.Disposable;
import logger.EventLogger;

import java.io.IOException;
import java.util.ArrayList;
import java.util.concurrent.locks.ReentrantLock;
import java.util.function.Predicate;

public class ConnectionHandler implements Observer<ResponseObject> {

    private final ReentrantLock lock = new ReentrantLock();
    private final ArrayList<Connection> connections = new ArrayList<>();

    @Override
    public void onSubscribe(@NonNull Disposable d) {

    }

    @Override
    public void onNext(@NonNull ResponseObject responseObject) {
        lock.lock();
        try{
            var predicate = new Predicate<Connection>() {
                @Override
                public boolean test(Connection connection) {
                    boolean isRight;
                    if (responseObject.requestType.equals("StartLiveAnalysis")) {
                        isRight = connection.getUserId().contains(responseObject.userId);
                        if (isRight) {
                            EventLogger.getLogger().info("Broadcasting server information to connections with client id" + responseObject.userId);
                        }
                    } else {
                        isRight = connection.getConnectionId().contains(responseObject.connectionId);
                        if (isRight) {
                            EventLogger.getLogger().info("Broadcasting server information to connections with connection id" + responseObject.connectionId);
                        }
                    }
                    return isRight;
                }
            };

            connections.stream().filter(predicate).forEach(
                    item -> {
                        try {
                            item.getWriter().append(responseObject.data).append("\n").flush();
                        } catch (IOException e) {
                            EventLogger.getLogger().logException(e);
                        }
                    }
                    );

        } finally {
            lock.unlock();
        }
    }

    @Override
    public void onError(@NonNull Throwable e) {
        EventLogger.getLogger().logException(e);
    }

    @Override
    public void onComplete() {

    }

    public void addConnection(Connection connection) {
        lock.lock();
        try {
            connections.add(connection);
        } finally {
            lock.unlock();
        }
    }

    public void removeConnection(String connectionId) {
        lock.lock();
        try {
            EventLogger.getLogger().info("Removing connection " + connectionId + " from ConnectionHandler");
            connections.removeIf(item -> item.getConnectionId().contains(connectionId));
        } finally {
            lock.unlock();
        }
    }
}
