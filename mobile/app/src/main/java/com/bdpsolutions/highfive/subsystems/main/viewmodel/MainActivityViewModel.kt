package com.bdpsolutions.highfive.subsystems.main.viewmodel

import android.Manifest
import android.util.Log
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ContextHolder
import dji.common.error.DJIError
import dji.common.error.DJISDKError
import dji.log.DJILog
import dji.sdk.base.BaseComponent
import dji.sdk.base.BaseProduct
import dji.sdk.sdkmanager.DJISDKInitEvent
import dji.sdk.sdkmanager.DJISDKManager
import io.reactivex.rxjava3.core.Observer
import java.util.ArrayList
import java.util.concurrent.atomic.AtomicBoolean

class MainActivityViewModel private constructor(): ViewModel() {

    private val TAG: String = MainActivityViewModel::class.java.name
    private val REQUEST_PERMISSION_CODE = 12345
    private val REQUIRED_PERMISSION_LIST = arrayOf(
        Manifest.permission.VIBRATE,
        Manifest.permission.INTERNET,
        Manifest.permission.ACCESS_WIFI_STATE,
        Manifest.permission.WAKE_LOCK,
        Manifest.permission.ACCESS_COARSE_LOCATION,
        Manifest.permission.ACCESS_NETWORK_STATE,
        Manifest.permission.ACCESS_FINE_LOCATION,
        Manifest.permission.CHANGE_WIFI_STATE,
        Manifest.permission.WRITE_EXTERNAL_STORAGE,
        Manifest.permission.BLUETOOTH,
        Manifest.permission.BLUETOOTH_ADMIN,
        Manifest.permission.READ_EXTERNAL_STORAGE,
        Manifest.permission.READ_PHONE_STATE
    )
    private val missingPermission: MutableList<String> = ArrayList()
    private val isRegistrationInProgress = AtomicBoolean(false)

    fun startSDKRegistration(observer: Observer<String>) {

        if (isRegistrationInProgress.compareAndSet(false, true)) {
            ConcurrencyExecutor.execute {
                ContextHolder.appContext?.let {
                    observer.onNext(it.getString(R.string.register_string))
                }
                DJISDKManager.getInstance().registerApp(ContextHolder.appContext?.applicationContext, object :
                    DJISDKManager.SDKManagerCallback {
                    override fun onRegister(djiError: DJIError) {
                        if (djiError === DJISDKError.REGISTRATION_SUCCESS) {
                            DJILog.e(
                                "App registration",
                                DJISDKError.REGISTRATION_SUCCESS.description
                            )
                            DJISDKManager.getInstance().startConnectionToProduct()
                            ContextHolder.appContext?.let {
                                observer.onNext(it.getString(R.string.register_success))
                            }
                        } else {
                            ContextHolder.appContext?.let {
                                observer.onNext(it.getString(R.string.register_failed))
                            }
                        }
                        Log.v(
                            TAG,
                            djiError.description
                        )
                    }

                    override fun onProductDisconnect() {
                        Log.d(
                            TAG,
                            "onProductDisconnect"
                        )
                        ContextHolder.appContext?.let {
                            observer.onNext(it.getString(R.string.drone_disconnected))
                        }
                    }

                    override fun onProductConnect(baseProduct: BaseProduct) {
                        Log.d(
                            TAG,
                            String.format("onProductConnect newProduct:%s", baseProduct)
                        )
                        ContextHolder.appContext?.let {
                            observer.onNext(it.getString(R.string.drone_connected))
                        }
                    }

                    override fun onProductChanged(baseProduct: BaseProduct) {}
                    override fun onComponentChange(
                        componentKey: BaseProduct.ComponentKey?, oldComponent: BaseComponent?,
                        newComponent: BaseComponent?
                    ) {
                        newComponent?.setComponentListener { isConnected ->
                            Log.d(
                                TAG,
                                "onComponentConnectivityChanged: $isConnected"
                            )
                        }
                        Log.d(
                            TAG, String.format(
                                "onComponentChange key:%s, oldComponent:%s, newComponent:%s",
                                componentKey,
                                oldComponent,
                                newComponent
                            )
                        )
                    }

                    override fun onInitProcess(djisdkInitEvent: DJISDKInitEvent, i: Int) {}
                    override fun onDatabaseDownloadProgress(l: Long, l1: Long) {}
                })
            }
        }
    }

    companion object {
        fun create(): MainActivityViewModel {
            return MainActivityViewModel()
        }
    }
}