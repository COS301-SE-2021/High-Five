package com.bdpsolutions.highfive.subsystems.drone

import android.content.Intent
import android.os.Bundle
import android.view.*
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.databinding.DroneFragmentBinding
import com.bdpsolutions.highfive.subsystems.drone.model.LiveStreamSocket
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import io.reactivex.rxjava3.core.Observer
import io.reactivex.rxjava3.disposables.Disposable
import java.net.URI

class DroneNavigationFragment: Fragment(){

    private var binding: DroneFragmentBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        binding = DroneFragmentBinding.inflate(layoutInflater)

        binding?.btnLiveStream?.setOnClickListener {
            startDroneActivity("StartLiveStream")
        }

        binding?.btnLiveAnalysis?.setOnClickListener {
            startDroneActivity("StartLiveAnalysis")
        }

        return binding?.root


    }

    private fun startDroneActivity(requestType: String) {
        val switchActivityIntent = Intent(activity, DroneActivity::class.java);
        switchActivityIntent.putExtra("requestType", requestType)
        startActivity(switchActivityIntent)
    }
}