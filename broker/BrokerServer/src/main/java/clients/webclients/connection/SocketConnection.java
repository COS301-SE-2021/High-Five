package clients.webclients.connection;

import org.apache.commons.io.IOUtil;

import java.io.*;
import java.net.Socket;

/**
 * Implementation class for the Connection interface. Uses a Java Socket for communication.
 *
 * @see Connection
 */
public class SocketConnection implements Connection{

    private final Socket connection;
    private String connectionId;
    private String userId;

    public SocketConnection(Socket connection) {
        this.connection = connection;
    }

    @Override
    public BufferedReader getReader() throws IOException {
        return new BufferedReader(new InputStreamReader(connection.getInputStream()));
    }

    @Override
    public BufferedWriter getWriter() throws IOException {
        return new BufferedWriter(new OutputStreamWriter(
                connection.getOutputStream()));
    }

    @Override
    public void close() throws IOException {
        connection.close();
    }

    @Override
    public boolean isConnected() {
        return connection.isConnected();
    }

    @Override
    public String getConnectionId() {
        return connectionId;
    }

    @Override
    public void setConnectionId(String connectionId) {
        this.connectionId = connectionId;
    }

    @Override
    public String getUserId() {
        return userId;
    }

    @Override
    public void setUserId(String userId) {
        this.userId = userId;
    }
}
