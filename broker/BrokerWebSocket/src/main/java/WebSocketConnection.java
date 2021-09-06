import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.*;
import java.net.Socket;
import org.apache.commons.io.IOUtil;

/**
 * WebSocket class that is loaded into an embedded Tomcat server. This class acts as a middleman
 * between the web client and the Broker, forwarding requests and responses between them.
 */
@ServerEndpoint(value="/broker")
public class WebSocketConnection {

    private Socket connection;

    @OnOpen
    public void onOpen(Session session) throws IOException {
        int port = Integer.parseInt(System.getenv("BROKER_CLIENT_PORT"));
        connection = new Socket("localhost", port);
    }

    /**
     * Fetches a request from the client and passes the request to the Broker.
     * Reads a response from the Broker and passes the response to the client.
     *
     * @param session WebSocket session to fetch and send data
     * @param message THe message from the web client
     */
    @OnMessage
    public void onMessage(Session session, String message) throws IOException {

        //Send request to broker
        Writer serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
                connection.getOutputStream()));

        serverInfoRequest.append(message).append("\n").flush();

        //Read response from broker
        StringWriter infoDataWriter = new StringWriter();
        IOUtil.copy(connection.getInputStream(), infoDataWriter, "UTF-8");
        String infoData = infoDataWriter.toString();

        //Send response to web client
        session.getBasicRemote().sendText(infoData);
    }

    @OnClose
    public void onClose(Session session) throws IOException {
        connection.close();
    }

    @OnError
    public void onError(Session session, Throwable throwable) {}
}
