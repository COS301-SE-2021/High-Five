package servicelocator.wrappers;

import clients.servers.ServerParticipant;
import io.reactivex.rxjava3.core.Observer;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.util.List;

public class ServerParticipantWrapper {
    public static Thread get(Observer serverParticipantObserver, List topics)
            throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator.getInstance()
                .<Thread>createClass("ServerParticipantListener", Observer.class, List.class)
                .newInstance(serverParticipantObserver, topics);
    }
}
