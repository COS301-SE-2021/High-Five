import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.*;
import java.net.Socket;
import java.net.SocketException;

/**
 * WebSocket class that is loaded into an embedded Tomcat server. This class acts as a middleman
 * between the web client and the Broker, forwarding requests and responses between them.
 */
@ServerEndpoint(value="/broker")
public class WebSocketConnection {

    private BufferedReader bufferedReader;
    private BufferedWriter serverInfoRequest;
    private Socket connection;
    private Thread responseThread;
    private boolean isRunning = true;
    private static final String CLOSECONNECTION = "closeconnection";

   @OnOpen
    public void onOpen(Session session) throws IOException {
       int port = Integer.parseInt(System.getenv("BROKER_CLIENT_PORT"));
       connection = new Socket("localhost", port);

       responseThread = new Thread(() -> {
           //Read response from broker
           while (isRunning) {
               try {
                   if (bufferedReader == null) {
                       bufferedReader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
                   }
                   String infoData = bufferedReader.readLine();
                   //Send response to web client
                   if (infoData != null) {
                       session.getBasicRemote().sendText(infoData);
                   }
               } catch (Exception ignored) {
                   return;
               }
           }
       });
       responseThread.start();
       serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
               connection.getOutputStream()));
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
        serverInfoRequest.append(message).append("\n").flush();
    }

    @OnClose
    public void onClose(Session session) throws IOException {
        serverInfoRequest.append(CLOSECONNECTION).append("\n").flush();
        try {
            connection.close();
        } catch (Exception ignored){}
        isRunning = false;
        try {
            responseThread.join();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }


    @OnError
    public void onError(Session session, Throwable throwable) throws IOException {
        serverInfoRequest.append(CLOSECONNECTION).append("\n").flush();
        try {
            connection.close();
        } catch (Exception ignored){}
        isRunning = false;
        try {
            responseThread.join();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
