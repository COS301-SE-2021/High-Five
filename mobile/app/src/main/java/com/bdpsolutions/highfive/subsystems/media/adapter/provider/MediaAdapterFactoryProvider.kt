package com.bdpsolutions.highfive.subsystems.media.adapter.provider

import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapter
import com.bdpsolutions.highfive.subsystems.media.adapter.MediaAdapterImpl
import com.bdpsolutions.highfive.subsystems.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.subsystems.video.adapter.VideoFragmentRecyclerViewAdapterImpl
import dagger.Binds
import dagger.Module
import dagger.hilt.InstallIn
import dagger.hilt.android.components.ActivityComponent

@Module
@InstallIn(ActivityComponent::class)
/**
 * This class provides the implementation for the MediaAdapter.
 *
 * Implementations are chosen here.
 */
abstract class MediaAdapterFactoryProvider {
    @Binds
    abstract fun bindMediaAdapterFactory(
        mediaAdapterImpl: MediaAdapterFactoryImpl
    ): MediaAdapterFactory
}