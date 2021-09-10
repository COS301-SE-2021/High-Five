package servicelocator.wrappers;

import clients.servers.ServerParticipant;
import dataclasses.serverinfo.ServerTopics;
import io.reactivex.rxjava3.core.Observer;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.util.List;

public class ServerParticipantWrapper {
    public static Thread get(Observer serverParticipantObserver, ServerTopics topics)
            throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator.getInstance()
                .<Thread>createClass("ServerParticipantListener", Observer.class, ServerTopics.class)
                .newInstance(serverParticipantObserver, topics);
    }
}
