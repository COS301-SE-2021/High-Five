package com.bdpsolutions.highfive.subsystems.drone

import android.content.Intent
import android.os.Bundle
import android.view.*
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.databinding.DroneFragmentBinding

class DroneNavigationFragment: Fragment(){

    private var binding: DroneFragmentBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        binding = DroneFragmentBinding.inflate(layoutInflater)

        binding?.btnLiveStream?.setOnClickListener {
            //Make call to broker to do live stream

            val switchActivityIntent = Intent(activity, DroneActivity::class.java);
            startActivity(switchActivityIntent)
        }

        binding?.btnLiveAnalysis?.setOnClickListener {
            val switchActivityIntent = Intent(activity, DroneActivity::class.java);
            startActivity(switchActivityIntent)
        }

        return binding?.root


    }




}