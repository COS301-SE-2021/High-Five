package com.bdpsolutions.highfive.subsystems.drone

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.view.View
import android.view.Window
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.ActivityDroneBinding
import com.bdpsolutions.highfive.databinding.ActivityLoginBinding
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.main.MainActivity
import com.bdpsolutions.highfive.utils.ContextHolder
import javax.inject.Inject

class DroneActivity : AppCompatActivity() {
    private lateinit var binding: ActivityDroneBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
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
                    Manifest.permission.READ_PHONE_STATE
                ), 1
            )
        }

        binding = ActivityDroneBinding.inflate(layoutInflater)
        setContentView(binding.root)
    }
}