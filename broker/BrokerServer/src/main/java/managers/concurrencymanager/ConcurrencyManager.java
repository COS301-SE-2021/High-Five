package managers.concurrencymanager;

import java.util.concurrent.Executor;
import java.util.concurrent.Executors;
import java.util.concurrent.locks.ReentrantLock;

public class ConcurrencyManager {
    private static ConcurrencyManager _instance;
    private final ReentrantLock lock = new ReentrantLock();
    private final Executor executor = Executors.newFixedThreadPool(100);

    private ConcurrencyManager() {}

    public void execute(Runnable runnable) {
        lock.lock();
        try {
            executor.execute(runnable);
        } finally {
            lock.unlock();
        }
    }

    public static ConcurrencyManager getInstance() {
        if (_instance == null) {
            _instance = new ConcurrencyManager();
        }
        return _instance;
    }
}
