package com.bdpsolutions.highfive.view.activities

import android.os.Bundle
import android.view.View
import com.google.android.material.bottomnavigation.BottomNavigationView
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.ActivityMainBinding
import com.google.android.material.navigation.NavigationView
import dagger.hilt.android.AndroidEntryPoint
import dagger.hilt.android.HiltAndroidApp
import androidx.appcompat.app.ActionBarDrawerToggle


@AndroidEntryPoint
class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.barLayout.toolbar)

        val mDrawerToggle = ActionBarDrawerToggle(
            this, binding.container, binding.barLayout.toolbar,
            R.string.open,R.string.close
        )
        binding.container.addDrawerListener(mDrawerToggle)
        supportActionBar!!.setDisplayHomeAsUpEnabled(true)
        supportActionBar!!.setHomeButtonEnabled(true)
        mDrawerToggle.syncState()




        val navView: NavigationView = binding.navView

        val navController = (supportFragmentManager.findFragmentById(
            R.id.nav_host_fragment_activity_main) as NavHostFragment).navController

        navView.setupWithNavController(navController)

    }
}