package com.bdpsolutions.highfive.subsystems.drone

import android.Manifest
import android.app.Application
import android.content.Context
import android.os.Handler
import android.os.Looper
import androidx.core.content.ContextCompat
import dji.sdk.base.BaseProduct
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.DJISDKManager.SDKManagerCallback

class DroneApplication: Application() {

    val FLAG_CONNECTION_CHANGE = "connection_change"

    private var mProduct: BaseProduct? = null
    private var mHandler: Handler? = null
    private var mDJISDKManagerCallback: SDKManagerCallback? = null

    private var instance: Application? = null

    fun setContext(application: Application?) {
        instance = application
    }

    override fun getApplicationContext(): Context? {
        return instance
    }

    fun DemoApplication() {}

    /**
     * This function is used to get the instance of DJIBaseProduct.
     * If no product is connected, it returns null.
     */
    @Synchronized
    fun getProductInstance(): BaseProduct? {
        if (null == mProduct) {
            mProduct = DJISDKManager.getInstance().product
        }
        return mProduct
    }

    override fun onCreate() {
        super.onCreate()
        mHandler = Handler(Looper.getMainLooper())

        //Check the permissions before registering the application for android system 6.0 above.
        val permissionCheck = ContextCompat.checkSelfPermission(
            applicationContext!!,
            Manifest.permission.WRITE_EXTERNAL_STORAGE
        )
        val permissionCheck2 = ContextCompat.checkSelfPermission(
            applicationContext!!,
            Manifest.permission.READ_PHONE_STATE
        )

    }
}