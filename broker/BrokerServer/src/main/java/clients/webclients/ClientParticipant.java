package clients.webclients;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.websocket.WebSocketConnection;

import java.io.IOException;
import java.util.LinkedList;

public class ClientParticipant extends WebClient{

    private WebSocketConnection connection;
    private final ServerInformationHolder informationHolder;

    public ClientParticipant(ServerInformationHolder informationHolder) {
        this.informationHolder = informationHolder;
    }

    @Override
    public boolean heartbeat() {
        return false;
    }

    @Override
    public void listen() throws InterruptedException {
        ServerInformation info = informationHolder.get();
        try {
            connection.sendServerInformation(info);
        } catch (IOException ignored) {}
    }
}
