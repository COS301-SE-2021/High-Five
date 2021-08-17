package com.bdpsolutions.highfive.subsystems.drone

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.databinding.AnalysisFragmentBinding
import com.bdpsolutions.highfive.subsystems.analysis.viewmodel.AnalysisViewModel

class DroneNavigationFragment: Fragment(){

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        var switchActivityIntent : Intent = Intent(activity, DroneActivity::class.java);
        startActivity(switchActivityIntent)
        return super.onCreateView(inflater, container, savedInstanceState)
    }




}