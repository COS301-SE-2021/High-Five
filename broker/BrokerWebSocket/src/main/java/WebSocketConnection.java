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

    private int state = 0; //Server gets Client ID first, therefore state is necessary to not close connection on first message
    private BufferedReader bufferedReader;
    private BufferedWriter serverInfoRequest;
    private Socket connection;

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

        if (state == 0) {
            connection = new Socket("localhost", port);
        }

        if (serverInfoRequest == null) {

            serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
                    connection.getOutputStream()));
        }

        serverInfoRequest.append(message).append("\n").flush();

        //Read response from broker
        if (bufferedReader == null) {
            bufferedReader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
        }
        String infoData = bufferedReader.readLine();

        //Send response to web client
        session.getBasicRemote().sendText(infoData);

        if (state == 1) {
            connection.close();
            connection = null;
            bufferedReader = null;
            serverInfoRequest = null;
            state = 0;
        } else {
            state++;
        }
    }

    @OnClose
    public void onClose(Session session) throws IOException {}


    @OnError
    public void onError(Session session, Throwable throwable) {}
}
