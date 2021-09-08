package servicelocator.wrappers;

import clients.webclients.connection.Connection;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.net.Socket;

public class ClientConnectionWrapper {
    public static Connection get(Socket connection) throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator.getInstance()
                .<Connection>createClass("ClientConnection", Socket.class)
                .newInstance(connection);
    }
}
