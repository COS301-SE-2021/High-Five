package com.bdpsolutions.highfive.view.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.databinding.FragmentVideoItemBinding

class VideoItemFragment: Fragment() {
    var binding: FragmentVideoItemBinding? = null
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentVideoItemBinding.inflate(layoutInflater)
        return binding?.root
    }
}