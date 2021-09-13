package clients.webclients.connection;

import org.apache.commons.io.IOUtil;

import java.io.*;
import java.net.Socket;
import java.util.concurrent.locks.ReentrantLock;

/**
 * Implementation class for the Connection interface. Uses a Java Socket for communication.
 *
 * @see Connection
 */
public class SocketConnection implements Connection{

    private final Socket connection;
    private String connectionId;
    private String userId;
    private BufferedReader connectionReader;
    private BufferedWriter connectionWriter;
    private final ReentrantLock lock = new ReentrantLock();

    public SocketConnection(Socket connection) {
        this.connection = connection;
    }

    @Override
    public BufferedReader getReader() throws IOException {
        lock.lock();
        try {
            if (connectionReader == null) {
                connectionReader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
            }
            return connectionReader;
        } finally {
            lock.unlock();
        }
    }

    @Override
    public BufferedWriter getWriter() throws IOException {
        lock.lock();
        try {
            if (connectionWriter == null) {
                connectionWriter = new BufferedWriter(new OutputStreamWriter(connection.getOutputStream()));
            }
            return connectionWriter;
        } finally {
            lock.unlock();
        }
    }

    @Override
    public void close() throws IOException {
        lock.lock();
        try {
            connection.close();
        } finally {
            lock.unlock();
        }
    }

    @Override
    public boolean isConnected() {
        lock.lock();
        try {
            return connection.isConnected();
        } finally {
            lock.unlock();
        }
    }

    @Override
    public String getConnectionId() {
        lock.lock();
        try {
            return connectionId;
        } finally {
            lock.unlock();
        }
    }

    @Override
    public void setConnectionId(String connectionId) {
        lock.lock();
        try {
            this.connectionId = connectionId;
        } finally {
            lock.unlock();
        }
    }

    @Override
    public String getUserId() {
        lock.lock();
        try {
            return userId;
        } finally {
            lock.unlock();
        }
    }

    @Override
    public void setUserId(String userId) {
        lock.lock();
        try {
            this.userId = userId;
        } finally {
            lock.unlock();
        }
    }
}
