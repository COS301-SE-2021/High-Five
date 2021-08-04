package com.bdpsolutions.highfive.view.fragments

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.bdpsolutions.highfive.databinding.AnalysisFragmentBinding
import com.bdpsolutions.highfive.viewmodel.analysis.AnalysisViewModel

class AnalysisFragment : Fragment() {

    private lateinit var viewModel: AnalysisViewModel


    private var _binding: AnalysisFragmentBinding? = null

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

        _binding = AnalysisFragmentBinding.inflate(inflater, container, false)
        val root: View = binding.root
        
        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

}