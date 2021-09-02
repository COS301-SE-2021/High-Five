import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.*;
import java.net.Socket;
import org.apache.commons.io.IOUtil;

@ServerEndpoint(
        value="/socket"
)
public class WebSocketConnection {

    private Session session;

    @OnOpen
    public void onOpen(Session session) throws IOException {
        this.session = session;
        System.out.println("Connection opened");
        // Get session and WebSocket connection
    }

    @OnMessage
    public void onMessage(Session session, String message) throws IOException {
        Socket brokerConnection = new Socket("localhost", 6666);
        Writer serverInfoRequest = new BufferedWriter(new OutputStreamWriter(
                brokerConnection.getOutputStream()));

        serverInfoRequest.append(message).flush();

        StringWriter infoDataWriter = new StringWriter();
        IOUtil.copy(brokerConnection.getInputStream(), infoDataWriter, "UTF-8");
        String infoData = infoDataWriter.toString();
        brokerConnection.close();

        session.getBasicRemote().sendText(infoData);

    }

    @OnClose
    public void onClose(Session session) throws IOException {
        // WebSocket connection closes
        System.out.println("Connection closed");
    }

    @OnError
    public void onError(Session session, Throwable throwable) {
        // Do error handling here
    }

}
