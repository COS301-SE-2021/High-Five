package clients.webclients;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;

import java.io.*;
import java.net.Socket;

public class ClientParticipant extends WebClient{

    private Socket connection;
    private final ServerInformationHolder informationHolder;

    public ClientParticipant(Socket connection, ServerInformationHolder informationHolder) {
        this.informationHolder = informationHolder;
        this.connection = connection;
    }

    @Override
    public boolean heartbeat() {
        return false;
    }

    @Override
    public void listen() throws InterruptedException {
        ServerInformation info = informationHolder.get();
        String infoString;
        if (info == null) {
            infoString = "No servers are available";
        } else {
            infoString = info.toString();
        }
        try {
            Writer out = new BufferedWriter(new OutputStreamWriter(
                    connection.getOutputStream()));

            out.append(infoString).flush();
            connection.close();
        } catch (IOException ignored) {}
    }
}
