package com.bdpsolutions.highfive.view.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.adapters.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.databinding.FragmentVideoBinding
import com.bdpsolutions.highfive.view.views.VideoItemView
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject

@AndroidEntryPoint
class VideoFragment: Fragment() {

    @Inject
    lateinit var service: VideoFragmentRecyclerViewAdapter

    var binding: FragmentVideoBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentVideoBinding.inflate(layoutInflater)
        val adapter = service
        binding?.videoHolder?.adapter = adapter
        adapter.setData(
            arrayOf(
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
            )
        )
        return binding?.root
        //return inflater.inflate(R.layout.fragment_video, container, false)
    }
}