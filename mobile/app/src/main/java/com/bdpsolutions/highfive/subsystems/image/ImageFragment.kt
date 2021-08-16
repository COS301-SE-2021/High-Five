package com.bdpsolutions.highfive.subsystems.image

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.provider.MediaStore
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.animation.Animation
import android.view.animation.AnimationUtils
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.databinding.ImageFragmentBinding
import com.bdpsolutions.highfive.subsystems.image.adapter.ImageRecyclerViewAdapter
import com.bdpsolutions.highfive.subsystems.image.view.ImageItemView
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageViewModel
import com.bdpsolutions.highfive.subsystems.video.view.VideoItemView
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import com.bdpsolutions.highfive.utils.Result
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject

@AndroidEntryPoint
class ImageFragment : Fragment() {

    private val rotateOpenAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.rotate_open_anim) }
    private val rotateCloseAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.rotate_close_anim) }
    private val fromBottomAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.from_bottom_anim) }
    private val fromTopAnim: Animation by lazy { AnimationUtils.loadAnimation(this.context, R.anim.from_top_anim) }

    private lateinit var viewModel: ImageViewModel
    private var binding: ImageFragmentBinding? = null
    private var clicked = false

    @Inject
    lateinit var adapter: ImageRecyclerViewAdapter
    @Inject
    lateinit var factory: ViewModelProvider.Factory

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = ImageFragmentBinding.inflate(layoutInflater)

        viewModel = ViewModelProvider(this, factory)
            .get(ImageViewModel::class.java)

        binding?.recyclerView?.adapter = adapter

        viewModel.registerFetchFromGallery(this)
        viewModel.registerAccessStoragePermission(this) {
            viewModel.launchGalleryChooser(
                Intent(
                    Intent.ACTION_PICK,
                    MediaStore.Images.Media.INTERNAL_CONTENT_URI
                )
            )
        }

        viewModel.imageResult.observe(viewLifecycleOwner, Observer {
            val imageResult = it ?: return@Observer

            if (imageResult.error != null) {
                Toast.makeText(context, "Failed to fetch data", Toast.LENGTH_LONG).show()
            } else {
                when(imageResult.success) {
                    is Result.Success<*> -> {
                        val itemViews = ArrayList<ImageItemView>()
                        for (item in imageResult.success.getResult().images!!) {
                            itemViews.add(
                                ImageItemView(
                                    imageName = item.name!!,
                                    imageId = item.id!!,
                                    imageDate = item.dateStored!!.toString(),
                                    imageUrl = item.url!!
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

        binding?.uploadImageBtn?.setOnClickListener {
            if (!clicked) {
                binding?.addFromCamera?.visibility = View.VISIBLE
                binding?.addFromCamera?.startAnimation(fromBottomAnim)
                binding?.addFromGallery?.visibility = View.VISIBLE
                binding?.addFromGallery?.startAnimation(fromBottomAnim)
                binding?.uploadImageBtn?.startAnimation(rotateOpenAnim)
            } else {
                binding?.addFromCamera?.visibility = View.INVISIBLE
                binding?.addFromCamera?.startAnimation(fromTopAnim)
                binding?.addFromGallery?.visibility = View.INVISIBLE
                binding?.addFromGallery?.startAnimation(fromTopAnim)
                binding?.uploadImageBtn?.startAnimation(rotateCloseAnim)
            }
            clicked = !clicked
        }

        binding?.addFromGallery?.setOnClickListener {
            if(ContextCompat.checkSelfPermission(this.requireContext(), Manifest.permission.READ_EXTERNAL_STORAGE)
                != PackageManager.PERMISSION_GRANTED)
            {
                viewModel.askPermission(Manifest.permission.READ_EXTERNAL_STORAGE)
            } else {
                viewModel.launchGalleryChooser(
                    Intent(
                        Intent.ACTION_PICK,
                        MediaStore.Images.Media.INTERNAL_CONTENT_URI
                    )
                )
            }
        }

        viewModel.fetchVideoData()

        return binding?.root
    }

}