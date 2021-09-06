package clients.webclients.connection;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.Reader;
import java.io.Writer;

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
    Writer getWriter() throws IOException;

    /**
     * Closes the connection
     */
    void close() throws IOException;

    boolean isConnected();
}
