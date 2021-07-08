package com.bdpsolutions.highfive.view.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil.setContentView
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.ActivityMainBinding
import com.bdpsolutions.highfive.databinding.FragmentVideoBinding
import com.bdpsolutions.highfive.utils.adapters.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.view.views.VideoItemView

class VideoFragment : Fragment() {

    var binding: FragmentVideoBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentVideoBinding.inflate(layoutInflater)
        binding?.videoHolder?.adapter = VideoFragmentRecyclerViewAdapter(arrayOf(
            VideoItemView(
                "Hello",
                "1",
                "2021-01-01"
            ),
            VideoItemView(
                "World",
                "2",
                "2021-01-02"
            )
        ))
        return binding?.root
        //return inflater.inflate(R.layout.fragment_video, container, false)
    }
}