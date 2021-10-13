package clients;

public interface ClientBase {
    boolean heartbeat();
    void listen() throws InterruptedException;
    void terminate();
}
