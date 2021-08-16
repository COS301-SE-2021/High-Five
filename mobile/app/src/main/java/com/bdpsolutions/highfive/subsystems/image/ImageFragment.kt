package com.bdpsolutions.highfive.subsystems.image

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.bdpsolutions.highfive.databinding.ImageFragmentBinding
import com.bdpsolutions.highfive.subsystems.image.adapter.ImageRecyclerViewAdapter
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageViewModel
import javax.inject.Inject

class ImageFragment : Fragment() {

    private lateinit var viewModel: ImageViewModel
    private var binding: ImageFragmentBinding? = null

    @Inject
    lateinit var adapter: ImageRecyclerViewAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = ImageFragmentBinding.inflate(layoutInflater)

        binding?.recyclerView?.adapter = adapter

        return binding?.root
    }
}