package com.bdpsolutions.highfive.subsystems.drone

import android.content.Intent
import android.os.Bundle
import android.view.*
import androidx.fragment.app.Fragment

class DroneNavigationFragment: Fragment(){

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val switchActivityIntent = Intent(activity, DroneActivity::class.java);
        startActivity(switchActivityIntent)
        return super.onCreateView(inflater, container, savedInstanceState)
    }




}