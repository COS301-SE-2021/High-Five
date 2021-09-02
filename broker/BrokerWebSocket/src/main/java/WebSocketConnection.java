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


    @OnOpen
    public void onOpen(Session session) throws IOException {}

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
        Socket brokerConnection = new Socket("localhost", 6666);
        Writer serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
                brokerConnection.getOutputStream()));

        serverInfoRequest.append(message).flush();

        //Read response from broker
        StringWriter infoDataWriter = new StringWriter();
        IOUtil.copy(brokerConnection.getInputStream(), infoDataWriter, "UTF-8");
        String infoData = infoDataWriter.toString();
        brokerConnection.close();

        //Send response to web client
        session.getBasicRemote().sendText(infoData);
    }

    @OnClose
    public void onClose(Session session) throws IOException {}

    @OnError
    public void onError(Session session, Throwable throwable) {}
}
