package clients.webclients;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;

import java.io.*;
import java.net.Socket;

public class ClientParticipant extends WebClient{

    private Socket connection;
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
            Writer out = new BufferedWriter(new OutputStreamWriter(
                    connection.getOutputStream()));

            out.append(info.toString()).flush();
            connection.close();
        } catch (IOException ignored) {}
    }
}
