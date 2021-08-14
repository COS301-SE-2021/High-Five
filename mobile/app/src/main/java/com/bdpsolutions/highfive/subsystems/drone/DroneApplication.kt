package com.bdpsolutions.highfive.subsystems.drone

import android.Manifest
import android.app.Application
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.widget.Toast
import androidx.core.content.ContextCompat
import dji.common.error.DJIError
import dji.common.error.DJISDKError
import dji.sdk.base.BaseComponent
import dji.sdk.base.BaseProduct
import dji.sdk.base.BaseProduct.ComponentKey
import dji.sdk.sdkmanager.DJISDKInitEvent
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

        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M || permissionCheck == 0 && permissionCheck2 == 0) {

            //This is used to start SDK services and initiate SDK.
            DJISDKManager.getInstance().registerApp(applicationContext, mDJISDKManagerCallback)
        } else {
            Toast.makeText(
                applicationContext,
                "Please check if the permission is granted.",
                Toast.LENGTH_LONG
            ).show()
        }

        /**
         * When starting SDK services, an instance of interface DJISDKManager.DJISDKManagerCallback will be used to listen to
         * the SDK Registration result and the product changing.
         */
        mDJISDKManagerCallback = object : SDKManagerCallback {
            //Listens to the SDK registration result
            override fun onRegister(error: DJIError) {
                if (error === DJISDKError.REGISTRATION_SUCCESS) {
                    DJISDKManager.getInstance().startConnectionToProduct()
                    val handler = Handler(Looper.getMainLooper())
                    handler.post {
                        Toast.makeText(applicationContext, "Register Success", Toast.LENGTH_LONG)
                            .show()
                    }
                } else {
                    val handler = Handler(Looper.getMainLooper())
                    handler.post {
                        Toast.makeText(
                            applicationContext,
                            "Register Failed, check network is available",
                            Toast.LENGTH_LONG
                        ).show()
                    }
                }
                Log.e("TAG", error.toString())
            }
            override fun onProductDisconnect() {
                Log.d("TAG", "onProductDisconnect")
                notifyStatusChange()
            }

            override fun onProductConnect(baseProduct: BaseProduct?) {
                Log.d("TAG", String.format("onProductConnect newProduct:%s", baseProduct))
                notifyStatusChange()
            }

            override fun onProductChanged(baseProduct: BaseProduct?) {
                Log.d("TAG", String.format("onProductChanged newProduct:%s", baseProduct))
                notifyStatusChange()
            }
            override fun onComponentChange(
                componentKey: ComponentKey?, oldComponent: BaseComponent?,
                newComponent: BaseComponent?
            ) {
                newComponent?.setComponentListener { isConnected ->
                    Log.d("TAG", "onComponentConnectivityChanged: $isConnected")
                    notifyStatusChange()
                }
                Log.d(
                    "TAG", String.format(
                        "onComponentChange key:%s, oldComponent:%s, newComponent:%s",
                        componentKey,
                        oldComponent,
                        newComponent
                    )
                )
            }
            override fun onInitProcess(djisdkInitEvent: DJISDKInitEvent?, i: Int) {}

            override fun onDatabaseDownloadProgress(l: Long, l1: Long) {}
        }
    }
    var updateRunnable = Runnable {
        val intent = Intent(FLAG_CONNECTION_CHANGE)
        applicationContext!!.sendBroadcast(intent)
    }

    fun notifyStatusChange() {
        mHandler!!.removeCallbacks(updateRunnable)
        mHandler!!.postDelayed(updateRunnable, 500)
    }

}