package com.bdpsolutions.highfive.subsystems.video.fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.animation.Animation
import android.view.animation.AnimationUtils
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.R
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

    private val rotateOpenAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.rotate_open_anim) }
    private val rotateCloseAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.rotate_close_anim) }
    private val fromBottomAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.from_bottom_anim) }
    private val fromTopAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.from_top_anim) }

    @Inject
    lateinit var adapter: VideoFragmentRecyclerViewAdapter
    @Inject
    lateinit var factory: ViewModelProvider.Factory

    private var clicked = false

    var binding: FragmentVideoBinding? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentVideoBinding.inflate(layoutInflater)

        videoViewModel = ViewModelProvider(this, factory)
            .get(VideoViewModel::class.java)

        binding?.videoHolder?.adapter = adapter

        videoViewModel.videoResult.observe(viewLifecycleOwner, Observer {
            val videoResult = it ?: return@Observer


            if (videoResult.error != null) {
                Toast.makeText(context, "Failed to fetch data", Toast.LENGTH_LONG).show()
            } else {
                when(videoResult.success) {
                    is Result.Success<*> -> {
                        val itemViews = ArrayList<VideoItemView>()
                        for (item in videoResult.success.getResult().videos!!) {
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
                        binding?.videoProgress?.visibility = View.INVISIBLE
                    }
                    is Result.Error -> {
                        Toast.makeText(context, "Unable to fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        })

        binding?.uploadVideo?.setOnClickListener {
            if (!clicked) {
                binding?.addFromRecorder?.visibility = View.VISIBLE
                binding?.addFromRecorder?.startAnimation(fromBottomAnim)
                binding?.addFromVidlibrary?.visibility = View.VISIBLE
                binding?.addFromVidlibrary?.startAnimation(fromBottomAnim)
                binding?.uploadVideo?.startAnimation(rotateOpenAnim)
            } else {
                binding?.addFromRecorder?.visibility = View.INVISIBLE
                binding?.addFromRecorder?.startAnimation(fromTopAnim)
                binding?.addFromVidlibrary?.visibility = View.INVISIBLE
                binding?.addFromVidlibrary?.startAnimation(fromTopAnim)
                binding?.uploadVideo?.startAnimation(rotateCloseAnim)
            }
            clicked = !clicked
        }

        videoViewModel.fetchVideoData()

        return binding?.root
    }
}