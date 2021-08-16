package com.bdpsolutions.highfive.subsystems.image

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
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

    private lateinit var viewModel: ImageViewModel
    private var binding: ImageFragmentBinding? = null

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

        viewModel.fetchVideoData()

        return binding?.root
    }
}