package servicelocator.wrappers;

import io.reactivex.rxjava3.core.Observer;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;

public class ClientListenerWrapper {
    public static Thread get(Observer clientListenerObserver)
            throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator
                .getInstance().<Thread>createClass("ClientListener", Observer.class)
                .newInstance(clientListenerObserver);
    }
}
