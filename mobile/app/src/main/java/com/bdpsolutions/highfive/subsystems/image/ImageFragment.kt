package com.bdpsolutions.highfive.subsystems.image

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.databinding.ImageFragmentBinding
import com.bdpsolutions.highfive.subsystems.image.adapter.ImageRecyclerViewAdapter
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageViewModel
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import javax.inject.Inject

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
        })

        return binding?.root
    }
}