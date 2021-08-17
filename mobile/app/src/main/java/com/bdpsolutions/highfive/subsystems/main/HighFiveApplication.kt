package com.bdpsolutions.highfive.subsystems.main

import android.app.Application
import android.content.Context
import com.bdpsolutions.highfive.subsystems.drone.DroneApplication
import com.secneo.sdk.Helper
import dagger.hilt.android.HiltAndroidApp

@HiltAndroidApp
class HighFiveApplication : Application(){
    override fun attachBaseContext(base: Context?) {
        super.attachBaseContext(base)
        Helper.install(this@HighFiveApplication)
    }
}
