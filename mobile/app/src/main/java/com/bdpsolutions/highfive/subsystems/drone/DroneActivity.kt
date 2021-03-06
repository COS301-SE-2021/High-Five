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
import android.view.WindowInsets
import android.view.WindowManager
import android.widget.Button
import android.widget.Toast
import android.widget.ToggleButton
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
import dji.common.camera.SettingsDefinitions
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
import dji.sdk.sdkmanager.LiveStreamManager
import dji.sdk.sdkmanager.LiveStreamManager.LiveStreamVideoSource
import dji.sdk.sdkmanager.LiveStreamManager.OnLiveChangeListener
import java.util.ArrayList

import java.util.concurrent.atomic.AtomicBoolean
import javax.inject.Inject
import android.os.Bundle
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import io.reactivex.rxjava3.core.Observer
import io.reactivex.rxjava3.disposables.Disposable


class DroneActivity : AppCompatActivity() {
    private val TAG: String = DroneActivity::class.java.getName()
    private lateinit var binding: ActivityDroneBinding
    private var requestType: String? = null
    private val toastObserver: Observer<String>

    init {
        toastObserver = object :Observer<String> {
            override fun onSubscribe(d: Disposable) {
                TODO("Not yet implemented")
            }

            override fun onNext(t: String) {
                showToast(t)
            }

            override fun onError(e: Throwable) {
                TODO("Not yet implemented")
            }

            override fun onComplete() {
                TODO("Not yet implemented")
            }

        }
    }

    private fun showToast(toastMsg: String) {
        runOnUiThread { Toast.makeText(applicationContext, toastMsg, Toast.LENGTH_LONG).show() }
    }

    private var btnLive: ToggleButton? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val extras = intent.extras
        requestType = extras?.getString("requestType")

        // When the compile and target version is higher than 22, please request the
        // following permissions at runtime to ensure the
        // SDK work well.
        val djiStreamer = DjiStreamer(toastObserver)
        binding = ActivityDroneBinding.inflate(layoutInflater)
        setContentView(binding.root)
        // Hiding status bars, since this is the splash screen
        btnLive = binding.toggleLive
        binding.toggleLive.setOnClickListener{
            if(binding.toggleLive.isChecked){
                djiStreamer.setupLiveStream(requestType)
            }
        }

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.R) {
            window.insetsController?.hide(WindowInsets.Type.statusBars())
        } else {
            // Hiding status bar for older versions of Android.
            @Suppress("DEPRECATION")
            window.setFlags(
                WindowManager.LayoutParams.FLAG_FULLSCREEN,
                WindowManager.LayoutParams.FLAG_FULLSCREEN,
            )
        }

        // When the compile and target version is higher than 22, please request the
        // following permissions at runtime to ensure the
        // SDK work well.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            ActivityCompat.requestPermissions(
                this, arrayOf(
                    Manifest.permission.WRITE_EXTERNAL_STORAGE,
                    Manifest.permission.VIBRATE,
                    Manifest.permission.INTERNET,
                    Manifest.permission.ACCESS_WIFI_STATE,
                    Manifest.permission.WAKE_LOCK,
                    Manifest.permission.ACCESS_COARSE_LOCATION,
                    Manifest.permission.ACCESS_NETWORK_STATE,
                    Manifest.permission.ACCESS_FINE_LOCATION,
                    Manifest.permission.CHANGE_WIFI_STATE,
                    Manifest.permission.MOUNT_UNMOUNT_FILESYSTEMS,
                    Manifest.permission.READ_EXTERNAL_STORAGE,
                    Manifest.permission.SYSTEM_ALERT_WINDOW,
                    Manifest.permission.READ_PHONE_STATE,
                    Manifest.permission.RECORD_AUDIO
                ), 1
            )
        }
    }

}
