package listeners;

import io.reactivex.rxjava3.core.Observable;

import java.net.http.WebSocket;

public abstract class IClientListener {
    protected WebSocket socket;

    public IClientListener() {}

    public abstract void listen(Observable<WebSocket> newConnectionHandler);
}
