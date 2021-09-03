package clients.webclients.connection;

import org.apache.commons.io.IOUtil;

import java.io.*;
import java.net.Socket;

public class SocketConnection implements Connection{

    private final Socket connection;

    public SocketConnection(Socket connection) {
        this.connection = connection;
    }

    @Override
    public void setInputWriter(Writer inputWriter) throws IOException {
        IOUtil.copy(connection.getInputStream(), inputWriter, "UTF-8");
    }

    @Override
    public Writer getOutputWriter() throws IOException {
        return new BufferedWriter(new OutputStreamWriter(
                connection.getOutputStream()));
    }

    @Override
    public void close() throws IOException {
        connection.close();
    }
}
