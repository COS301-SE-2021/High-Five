package com.bdpsolutions.highfive.subsystems.media.adapter.provider

import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapter

interface MediaAdapterFactory {
    fun create(fragment: Fragment): MediaAdapter
}