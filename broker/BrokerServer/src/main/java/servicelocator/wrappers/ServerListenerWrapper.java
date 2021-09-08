package servicelocator.wrappers;

import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.util.List;

public class ServerListenerWrapper {
    public static Thread get(Observer serverObservable, List topics) throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator
                .getInstance().<Thread>createClass("ServerListener", Observer.class, List.class)
                .newInstance(serverObservable, topics);
    }
}
