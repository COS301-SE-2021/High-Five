package com.bdpsolutions.highfive.subsystems.main

import android.app.Application
import android.content.Context
import com.bdpsolutions.highfive.subsystems.drone.DroneApplication
import com.secneo.sdk.Helper
import dagger.hilt.android.HiltAndroidApp

@HiltAndroidApp
class HighFiveApplication : Application(){
    private var droneApplication: DroneApplication? = null



    override fun attachBaseContext(base: Context?) {
        super.attachBaseContext(base)
        Helper.install(this@HighFiveApplication)
        if (droneApplication == null) {
            droneApplication = DroneApplication()
            droneApplication!!.setContext(this)
        }
        app = this
    }
companion object{
    var app:Application? = null
    fun getInstance(): Application? {
        return app
    }
}


    override fun onCreate() {
        super.onCreate()
        droneApplication?.onCreate()
    }
}
