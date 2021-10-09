package managers.concurrencymanager;

import java.util.concurrent.Executor;
import java.util.concurrent.Executors;
import java.util.concurrent.locks.ReentrantLock;

/**
 * This class holds a thread pool used to execute various tasks asynchronously, such as
 * handling client connections, listening for new analysis engines, etc.
 *
 * This class is ultimately responsible for handling all asynchronous tasks.
 */
public class ConcurrencyManager {
    private static ConcurrencyManager _instance;
    private final ReentrantLock lock = new ReentrantLock();
    private final Executor executor = Executors.newFixedThreadPool(100);

    private ConcurrencyManager() {}

    /**
     * Accepts and executes the Runnable asynchronously.
     * @param runnable Task to be executed.
     */
    public void execute(Runnable runnable) {
        lock.lock();
        try {
            executor.execute(runnable);
        } finally {
            lock.unlock();
        }
    }

    /**
     * Creates an instance of this class if it hasn't been created already and returns
     * the instance.
     * @return Instance of the ConcurrencyManager class.
     */
    public static ConcurrencyManager getInstance() {
        if (_instance == null) {
            _instance = new ConcurrencyManager();
        }
        return _instance;
    }
}
