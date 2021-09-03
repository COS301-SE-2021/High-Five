package clients.webclients.connection;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.Writer;

public interface Connection {
    void setInputWriter(Writer inputWriter) throws IOException;
    Writer getOutputWriter() throws IOException;
    void close() throws IOException;
}
