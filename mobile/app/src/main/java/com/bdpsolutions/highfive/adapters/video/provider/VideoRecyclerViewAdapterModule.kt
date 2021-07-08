package com.bdpsolutions.highfive.adapters.video.provider

import com.bdpsolutions.highfive.adapters.video.adapter.VideoFragmentRecyclerViewAdapter
import com.bdpsolutions.highfive.adapters.video.adapter.VideoFragmentRecyclerViewAdapterImpl
import dagger.Binds
import dagger.Module
import dagger.hilt.InstallIn
import dagger.hilt.android.components.ActivityComponent

@Module
@InstallIn(ActivityComponent::class)
abstract class VideoRecyclerViewAdapterModule {
    @Binds
    abstract fun bindRecyclerView(
        recyclerViewImpl: VideoFragmentRecyclerViewAdapterImpl
    ): VideoFragmentRecyclerViewAdapter
}