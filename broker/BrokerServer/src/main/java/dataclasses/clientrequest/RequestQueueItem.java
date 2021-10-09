package dataclasses.clientrequest;

import clients.ClientBase;
import clients.webclients.connection.Connection;
import clients.webclients.connectionhandler.ConnectionHandler;
import dataclasses.serverinfo.ServerInformationHolder;

public class RequestQueueItem {
    public AnalysisRequest request;
    public ServerInformationHolder informationHolder;
    public Connection connection;
    public ConnectionHandler handler;
    public ClientBase client;
    public final long expires = System.currentTimeMillis() + 1800000; //Expires in 30 minutes

    public RequestQueueItem(AnalysisRequest request, ServerInformationHolder informationHolder, Connection connection, ConnectionHandler handler, ClientBase client) {
        this.connection = connection;
        this.handler = handler;
        this.request = request;
        this.informationHolder = informationHolder;
        this.client = client;
    }
}
