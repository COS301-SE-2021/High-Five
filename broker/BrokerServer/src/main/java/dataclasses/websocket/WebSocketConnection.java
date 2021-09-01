package dataclasses.websocket;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.websocket.codecs.*;

import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.IOException;

@ServerEndpoint(
        value="/broker/{clientType}",
        decoders = RequestDecoder.class,
        encoders = RequestEncoder.class )
public class WebSocketConnection {
    private Session session;

    @OnOpen
    public void onOpen(Session session) throws IOException {
        this.session = session;
        // Get session and WebSocket connection
    }

    @OnMessage
    public void onMessage(Session session, ClientRequest message) throws IOException {
        // Handle new messages
    }

    @OnClose
    public void onClose(Session session) throws IOException {
        // WebSocket connection closes
    }

    @OnError
    public void onError(Session session, Throwable throwable) {
        // Do error handling here
    }

    public void sendServerInformation(ServerInformation information) throws IOException {
        session.getBasicRemote().sendText(information.toString());
    }
}
