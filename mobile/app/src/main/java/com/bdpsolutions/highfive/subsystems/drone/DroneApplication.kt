package com.bdpsolutions.highfive.subsystems.drone

import android.app.Application
import android.content.Context
import android.os.Handler
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
}