package com.bdpsolutions.highfive.subsystems.drone

import android.Manifest
import android.app.Activity
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.os.*
import android.util.Log
import android.view.View
import android.view.Window
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.ActivityDroneBinding
import com.bdpsolutions.highfive.databinding.ActivityLoginBinding
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.main.MainActivity
import com.bdpsolutions.highfive.utils.ContextHolder
import dji.common.error.DJIError
import dji.common.error.DJISDKError
import dji.log.DJILog
import dji.sdk.base.BaseComponent
import dji.sdk.base.BaseProduct
import dji.sdk.base.BaseProduct.ComponentKey
import dji.sdk.products.Aircraft
import dji.sdk.sdkmanager.DJISDKInitEvent
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.DJISDKManager.SDKManagerCallback
import java.util.ArrayList

import java.util.concurrent.atomic.AtomicBoolean
import javax.inject.Inject

class DroneActivity : AppCompatActivity() {
    private val TAG: String = DroneActivity::class.java.getName()
    private lateinit var binding: ActivityDroneBinding

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

    private fun showToast(toastMsg: String) {
        runOnUiThread { Toast.makeText(applicationContext, toastMsg, Toast.LENGTH_LONG).show() }
    }
    private fun startSDKRegistration() {
        if (isRegistrationInProgress.compareAndSet(false, true)) {
            AsyncTask.execute {
                showToast("registering, pls wait...")
                DJISDKManager.getInstance().registerApp(applicationContext, object : SDKManagerCallback {
                        override fun onRegister(djiError: DJIError) {
                            if (djiError === DJISDKError.REGISTRATION_SUCCESS) {
                                DJILog.e(
                                    "App registration",
                                    DJISDKError.REGISTRATION_SUCCESS.description
                                )
                                DJISDKManager.getInstance().startConnectionToProduct()
                                showToast("Register Success")
                            } else {
                                showToast("Register sdk fails, check network is available")
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
                            showToast("Product Disconnected")
                        }

                        override fun onProductConnect(baseProduct: BaseProduct) {
                            Log.d(
                                TAG,
                                String.format("onProductConnect newProduct:%s", baseProduct)
                            )
                            showToast("Product Connected")
                        }

                        override fun onProductChanged(baseProduct: BaseProduct) {}
                        override fun onComponentChange(
                            componentKey: ComponentKey, oldComponent: BaseComponent,
                            newComponent: BaseComponent
                        ) {
                            if (newComponent != null) {
                                newComponent.setComponentListener { isConnected ->
                                    Log.d(
                                        TAG,
                                        "onComponentConnectivityChanged: $isConnected"
                                    )
                                }
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

    /**
     * Checks if there is any missing permissions, and
     * requests runtime permission if needed.
     */
    private fun checkAndRequestPermissions() {
        // Check for permissions
        for (eachPermission in REQUIRED_PERMISSION_LIST) {
            if (ContextCompat.checkSelfPermission(
                    this,
                    eachPermission
                ) != PackageManager.PERMISSION_GRANTED) {
                missingPermission.add(eachPermission)
            }
        }
        // Request for missing permissions
        if (!missingPermission.isEmpty() && Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            ActivityCompat.requestPermissions(
                this,
                missingPermission.toTypedArray(),
                REQUEST_PERMISSION_CODE
            )
        }
    }

    /**
     * Result of runtime permission request
     */
    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<String?>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        // Check for granted permission and remove from missing list
        if (requestCode == REQUEST_PERMISSION_CODE) {
            for (i in grantResults.indices.reversed()) {
                if (grantResults[i] == PackageManager.PERMISSION_GRANTED) {
                    missingPermission.remove(permissions[i])
                }
            }
        }
        // If there is enough permission, we will start the registration
        if (missingPermission.isEmpty()) {
            startSDKRegistration()
        } else {
            showToast("Missing permissions!!!")
        }
    }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        checkAndRequestPermissions()
        // When the compile and target version is higher than 22, please request the
        // following permissions at runtime to ensure the
        // SDK work well.


        binding = ActivityDroneBinding.inflate(layoutInflater)
        setContentView(binding.root)
    }


}