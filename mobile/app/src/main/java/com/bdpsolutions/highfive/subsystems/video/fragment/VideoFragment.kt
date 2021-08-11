package com.bdpsolutions.highfive.subsystems.video.fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.subsystems.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.databinding.FragmentVideoBinding
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.subsystems.video.view.VideoItemView
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject

@AndroidEntryPoint
class VideoFragment: Fragment() {

    private lateinit var videoViewModel: VideoViewModel

    @Inject
    lateinit var service: VideoFragmentRecyclerViewAdapter
    @Inject
    lateinit var factory: ViewModelProvider.Factory

    var binding: FragmentVideoBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentVideoBinding.inflate(layoutInflater)

        videoViewModel = ViewModelProvider(this, factory)
            .get(VideoViewModel::class.java)

        val adapter = service
        binding?.videoHolder?.adapter = adapter

        videoViewModel.videoResult.observe(viewLifecycleOwner, Observer {
            val videoResult = it ?: return@Observer


            if (videoResult.error != null) {
                Toast.makeText(context, "Failed to fetch data", Toast.LENGTH_LONG).show()
            } else {
                when(videoResult.success) {
                    is Result.Success<*> -> {
                        val itemViews = ArrayList<VideoItemView>()
                        for (item in videoResult.success.getResult()) {
                            itemViews.add(
                                VideoItemView(
                                    videoName = item.name!!,
                                    videoId = item.id!!,
                                    videoDate = item.dateStored!!.toString()
                                )
                            )
                        }
                        adapter.setData(itemViews.toTypedArray())
                        adapter.notifyDataSetChanged()
                    }
                    is Result.Error -> {
                        Toast.makeText(context, "Unable to fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        })

        videoViewModel.fetchVideoData()

        return binding?.root
        //return inflater.inflate(R.layout.fragment_video, container, false)
    }
}