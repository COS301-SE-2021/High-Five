package com.bdpsolutions.highfive.subsystems.media

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.viewpager2.widget.ViewPager2
import com.bdpsolutions.highfive.databinding.MediaFragmentBinding
import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapter
import com.bdpsolutions.highfive.subsystems.media.adapter.provider.MediaAdapterFactory
import com.bdpsolutions.highfive.subsystems.media.viewmodel.MediaViewModel
import com.google.android.material.tabs.TabLayoutMediator
import dagger.hilt.android.AndroidEntryPoint
import java.text.FieldPosition
import javax.inject.Inject

@AndroidEntryPoint
class MediaFragment : Fragment() {

    private lateinit var viewModel: MediaViewModel
    var binding: MediaFragmentBinding? = null

    @Inject
    lateinit var factory: ViewModelProvider.Factory

    @Inject
    lateinit var adapterFactory: MediaAdapterFactory

    private lateinit var mediaAdapter: MediaAdapter
    private lateinit var viewPager: ViewPager2


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = MediaFragmentBinding.inflate(layoutInflater)
        viewModel = ViewModelProvider(this, factory)
            .get(MediaViewModel::class.java)
        return binding?.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        val tabLayout = binding?.mediaFormats

        mediaAdapter = adapterFactory.create(this)
        viewPager = binding?.mediaPager!!
        viewPager.adapter = mediaAdapter

        TabLayoutMediator(tabLayout!!, viewPager) { tab, position ->
            tab.text = fetchTabString(position)
        }.attach()
    }

    private fun fetchTabString(position: Int) : String {
        return when (position) {
            0 -> "Images"
            1 -> "Videos"
            else -> ""
        }
    }
}