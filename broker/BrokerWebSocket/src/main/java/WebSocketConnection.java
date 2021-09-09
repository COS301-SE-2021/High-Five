import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.*;
import java.net.Socket;

/**
 * WebSocket class that is loaded into an embedded Tomcat server. This class acts as a middleman
 * between the web client and the Broker, forwarding requests and responses between them.
 */
@ServerEndpoint(value="/broker")
public class WebSocketConnection {

    @OnOpen
    public void onOpen(Session session) throws IOException {

    }

    /**
     * Fetches a request from the client and passes the request to the Broker.
     * Reads a response from the Broker and passes the response to the client.
     *
     * @param session WebSocket session to fetch and send data
     * @param message The message from the web client
     */
    @OnMessage
    public void onMessage(Session session, String message) throws IOException {
        //Send request to broker
        int port = Integer.parseInt(System.getenv("BROKER_CLIENT_PORT"));
        Socket connection = new Socket("localhost", port);
        Writer serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
                connection.getOutputStream()));

        serverInfoRequest.append(message).append("\n").flush();

        //Read response from broker
        BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
        String infoData = bufferedReader.readLine();

        //Send response to web client
        session.getBasicRemote().sendText(infoData);
        connection.close();
    }

    @OnClose
    public void onClose(Session session) throws IOException {}

    @OnError
    public void onError(Session session, Throwable throwable) {}
}
