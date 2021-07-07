package com.bdpsolutions.highfive.ui.splash

import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.WindowInsets
import android.view.WindowManager
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.databinding.ActivitySplashBinding
import com.bdpsolutions.highfive.ui.login.LoginViewModel
import com.bdpsolutions.highfive.ui.login.LoginViewModelFactory

class SplashActivity : AppCompatActivity() {

    private lateinit var splashViewModel: SplashViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val splashBinding: ActivitySplashBinding = ActivitySplashBinding.inflate(layoutInflater)
        setContentView(splashBinding.root)

        splashViewModel = ViewModelProvider(this, SplashViewModelFactory())
            .get(SplashViewModel::class.java)

        // Hiding status bars, since this is the splash screen
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
        splashViewModel.redirectActivity(this)
    }
}