package servicelocator.wrappers;

import clients.webclients.WebClient;
import clients.webclients.connection.Connection;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.ServerInformationHolder;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;

public class WebClientWrapper {
    public static WebClient get(Connection webConnection, ServerInformationHolder serverInformationHolder)
            throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator.getInstance()
                .<WebClient>createClass("ClientParticipant", Connection.class, ServerInformationHolder.class)
                .newInstance(webConnection, serverInformationHolder);
    }
}
