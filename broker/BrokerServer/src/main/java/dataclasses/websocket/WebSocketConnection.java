package dataclasses.websocket;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.websocket.codecs.*;

import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.IOException;

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
        System.out.println(message);
        // Handle new messages
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

    public void sendServerInformation(ServerInformation information) throws IOException {
        session.getBasicRemote().sendText(information.toString());
    }

    public void doOnOpen(AddConnection addConnection) {
        addConnection.operation(this);
    }

    public interface AddConnection {
        void operation(WebSocketConnection connection);
    }
}
