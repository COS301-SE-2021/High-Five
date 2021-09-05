package clients.webclients.connection;

import java.io.IOException;
import java.io.Writer;

/**
 * Interface for connections to a client. A connection could be over
 * any medium, and is up to the implementation class to handle communication.
 */
public interface Connection {

    /**
     * Accepts a Writer from which it collects information to be sent over the connection.
     *
     * @param inputWriter Writer to use for outgoing communication.
     * @throws IOException
     */
    void setInputWriter(Writer inputWriter) throws IOException;

    /**
     * Returns a Writer from which to read data coming in from the connection.
     *
     * @return Writer to use for incoming communication.
     */
    Writer getOutputWriter() throws IOException;

    /**
     * Closes the connection
     */
    void close() throws IOException;
}
