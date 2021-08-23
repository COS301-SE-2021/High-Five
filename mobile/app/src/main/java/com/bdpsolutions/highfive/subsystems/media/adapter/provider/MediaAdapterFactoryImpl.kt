package com.bdpsolutions.highfive.subsystems.media.adapter.provider

import androidx.fragment.app.Fragment
import com.bdpsolutions.highfive.constants.Exceptions.MEDIA_ADAPTER_FACTORY
import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapter
import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapterImpl
import javax.inject.Inject

class MediaAdapterFactoryImpl @Inject constructor() : MediaAdapterFactory {
    @Suppress("UNCHECKED_CAST")
    override fun create(fragment: Fragment): MediaAdapter {
        return MediaAdapterImpl(fragment)
    }
}