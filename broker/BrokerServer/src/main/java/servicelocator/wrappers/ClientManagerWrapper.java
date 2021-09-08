package servicelocator.wrappers;

import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.serverinfo.codecs.ServerInformationDecoder;
import managers.Manager;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;

public class ClientManagerWrapper {
    public static Manager get(ServerInformationHolder holder) throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator
                .getInstance()
                .<Manager>createClass("ClientManager", ServerInformationHolder.class)
                .newInstance(holder);
    }
}
