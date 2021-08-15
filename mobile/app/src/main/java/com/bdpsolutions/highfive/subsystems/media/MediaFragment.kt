package com.bdpsolutions.highfive.subsystems.media

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.FragmentVideoBinding
import com.bdpsolutions.highfive.databinding.MediaFragmentBinding
import com.bdpsolutions.highfive.subsystems.media.viewmodel.MediaViewModel

class MediaFragment : Fragment() {

    companion object {
        fun newInstance() = MediaFragment()
    }

    private lateinit var viewModel: MediaViewModel
    var binding: MediaFragmentBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = MediaFragmentBinding.inflate(layoutInflater)
        return binding?.root
    }

}