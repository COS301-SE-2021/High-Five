import dataclasses.serverinfo.*;
import logger.EventLogger;
import servicelocator.wrappers.*;

import java.lang.reflect.*;


/**
 * This class is essentially the glue that holds the entire Broker system together.
 * Instantiates listeners for new servers and clients, as well as creating observers
 * that pass messages between the components of the Broker system.
 */
public class Broker {
    private Thread serverListener;
    private Thread clientListener;
    private final ServerInformationHolder serverInformationHolder = new ServerInformationHolder();

    public Broker() {

        EventLogger.getLogger().info("Creating listener classes");
        try {

            clientListener = ClientManagerWrapper.get(serverInformationHolder);
            serverListener = ServerManagerWrapper.get(serverInformationHolder);
            clientListener.start();
            serverListener.start();

        } catch (InvocationTargetException | IllegalAccessException | InstantiationException e) {
            EventLogger.getLogger().logException(e);
        }
    }
}
