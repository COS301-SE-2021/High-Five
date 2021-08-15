package com.bdpsolutions.highfive.subsystems.media.adapter

import android.os.Bundle
import androidx.fragment.app.Fragment
import androidx.viewpager2.adapter.FragmentStateAdapter
import com.bdpsolutions.highfive.constants.Fragments
import com.bdpsolutions.highfive.subsystems.video.fragment.VideoFragment
import javax.inject.Inject

class MediaAdapterImpl(fragment: Fragment): MediaAdapter(fragment) {

    override fun getItemCount(): Int = 2

    override fun createFragment(position: Int): Fragment {
        // Return a NEW fragment instance in createFragment(int)
        val fragment = VideoFragment()
        fragment.arguments = Bundle().apply {
            // Our object is just an integer :-P
            putInt(Fragments.VIDEO_FRAGMENT, position + 1)
        }
        return fragment
    }
}

