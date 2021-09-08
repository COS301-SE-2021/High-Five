package servicelocator.wrappers;

import dataclasses.serverinfo.ServerInformationHolder;
import managers.Manager;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;

public class ServerManagerWrapper {
    public static Manager get(ServerInformationHolder holder) throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator
                .getInstance()
                .<Manager>createClass("ServerManager", ServerInformationHolder.class)
                .newInstance(holder);
    }
}
