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
                    if (responseObject.requestType.contains("StartLive")) { // Live analysis or live streaming
                        isRight = connection.getUserId().contains(responseObject.userId);
                        if (isRight) {
                            EventLogger.getLogger().info("Broadcasting server information to connections with client id " + responseObject.userId);
                        }
                    } else { // Uploaded media
                        isRight = connection.getConnectionId().contains(responseObject.connectionId);
                        if (isRight) {
                            EventLogger.getLogger().info("Broadcasting server information to connections with connection id " + responseObject.connectionId);
                        }
                    }
                    return isRight;
                }
            };

            ArrayList<String> connectionsToRemove = new ArrayList<>();

            connections.stream().filter(predicate).forEach(
                    item -> {
                        try {
                            item.getWriter().append(responseObject.data).append("\n").flush();
                        } catch (IOException e) {
                            EventLogger.getLogger().logException(e);
                            connectionsToRemove.add(item.getConnectionId());
                        }
                    }
                    );

            connectionsToRemove.forEach(this::_removeConnection);

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

    private void _removeConnection(String connectionId) {
        EventLogger.getLogger().info("Removing connection " + connectionId + " from ConnectionHandler");
        connections.removeIf(item -> item.getConnectionId().contains(connectionId));
    }

    public void removeConnection(String connectionId) {
        lock.lock();
        try {
            _removeConnection(connectionId);
        } finally {
            lock.unlock();
        }
    }

    public String getUserId(String connectionId) {
        lock.lock();
        try {
            return connections.stream()
                    .filter(item -> item.getConnectionId().contains(connectionId))
                    .findFirst()
                    .map(Connection::getUserId)
                    .orElse("");
        } finally {
            lock.unlock();
        }
    }
}
