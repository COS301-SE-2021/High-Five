package com.bdpsolutions.highfive.utils

import java.util.concurrent.*

/**
 * This class runs tasks given to it asynchronously. This is necessary for tasks such as database
 * operations and API requests. This allows such tasks to run without slowing down the UI of the
 * application.
 */
object ConcurrencyExecutor {
    private val executor: Executor = Executors.newFixedThreadPool(4)

    /**
     * Executes the given task
     *
     * @param task task to run
     */
    fun execute(task: () -> Unit) {
        executor.execute(task)
    }
}