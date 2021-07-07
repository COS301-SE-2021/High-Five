package com.bdpsolutions.highfive.ui.analysis

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.FragmentHomeBinding
import com.bdpsolutions.highfive.ui.home.HomeViewModel

class AnalysisFragment : Fragment() {

    private lateinit var viewModel: AnalysisViewModel


    private var _binding: FragmentHomeBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        viewModel =
            ViewModelProvider(this).get(AnalysisViewModel::class.java)

        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        val root: View = binding.root
        
        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

}