package servicelocator.wrappers;

import clients.webclients.WebClient;
import clients.webclients.connection.Connection;
import clients.webclients.connectionhandler.ConnectionHandler;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;

public class WebClientWrapper {
    public static WebClient get(Connection webConnection, ConnectionHandler handler, ServerInformationHolder serverInformationHolder)
            throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator.getInstance()
                .<WebClient>createClass("ClientParticipant", Connection.class, ConnectionHandler.class, ServerInformationHolder.class)
                .newInstance(webConnection, handler, serverInformationHolder);
    }
}
