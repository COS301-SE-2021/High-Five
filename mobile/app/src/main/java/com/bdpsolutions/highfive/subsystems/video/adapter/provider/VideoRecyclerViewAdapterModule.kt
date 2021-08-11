package com.bdpsolutions.highfive.subsystems.video.adapter.provider

import com.bdpsolutions.highfive.subsystems.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.subsystems.video.adapter.VideoFragmentRecyclerViewAdapterImpl
import dagger.Binds
import dagger.Module
import dagger.hilt.InstallIn
import dagger.hilt.android.components.ActivityComponent

@Module
@InstallIn(ActivityComponent::class)
/**
 * This class provides the implementation for the VideoFragmentRecyclerViewAdapter.
 *
 * Implementations are chosen here.
 */
abstract class VideoRecyclerViewAdapterModule {
    @Binds
    abstract fun bindRecyclerView(
        recyclerViewImpl: VideoFragmentRecyclerViewAdapterImpl
    ): VideoFragmentRecyclerViewAdapter
}