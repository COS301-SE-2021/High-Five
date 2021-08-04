package com.bdpsolutions.highfive.view.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.adapters.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.databinding.FragmentVideoBinding
import com.bdpsolutions.highfive.models.video.VideoDataRepository
import com.bdpsolutions.highfive.models.video.source.APIVideoDataSource
import com.bdpsolutions.highfive.utils.Result
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
        val repository = VideoDataRepository(APIVideoDataSource(), APIVideoDataSource())
        repository.refreshData {
            result -> run {
                when(result) {
                    is Result.Success<*> -> {
                        val itemViews = ArrayList<VideoItemView>()
                        for (item in result.getResult()) {
                            itemViews.add(
                                VideoItemView(
                                    videoName = item.name!!,
                                    videoId = item.id!!,
                                    videoDate = item.dateStored!!.toString()
                            ))
                        }
                        adapter.setData(itemViews.toTypedArray())
                        adapter.notifyDataSetChanged()
                    }
                    is Result.Error -> {
                        Toast.makeText(context, "Unable to fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }

        return binding?.root
        //return inflater.inflate(R.layout.fragment_video, container, false)
    }
}