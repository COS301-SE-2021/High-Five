package com.bdpsolutions.highfive.subsystems.drone

import android.app.Application
import android.content.Context

class DroneApplication: Application() {
    private var instance: Application? = null
    fun setContext(application: Application?) {
        instance = application
    }

    override fun getApplicationContext(): Context? {
        return instance
    }

    fun DemoApplication() {}
}