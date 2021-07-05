package com.high5.utils;

import java.util.concurrent.Executor;
import java.util.concurrent.Executors;

public class ThreadExecutor {
    private static volatile ThreadExecutor instance;
    private final Executor threadExecutor;

    private ThreadExecutor() {
        threadExecutor = Executors.newFixedThreadPool(4);
    }

    public static synchronized void execute(Runnable run) {
        if (instance == null) {
            instance = new ThreadExecutor();
        }
        instance.threadExecutor.execute(run);
    }
}
