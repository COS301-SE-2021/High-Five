package clients.webclients.connection;

import java.io.*;

/**
 * Interface for connections to a client. A connection could be over
 * any medium, and is up to the implementation class to handle communication.
 */
public interface Connection {

    /**
     * Returns a reader to read data from the connection
     *
     * @return Reader for reading data
     */
    BufferedReader getReader() throws IOException;

    /**
     * Returns a Writer from which to read data coming in from the connection.
     *
     * @return Writer to use for incoming communication.
     */
    BufferedWriter getWriter() throws IOException;

    /**
     * Closes the connection
     */
    void close() throws IOException;

    boolean isConnected();

    String getConnectionId();

    void setConnectionId(String connectionId);

    String getUserId();

    void setUserId(String userId);
}
