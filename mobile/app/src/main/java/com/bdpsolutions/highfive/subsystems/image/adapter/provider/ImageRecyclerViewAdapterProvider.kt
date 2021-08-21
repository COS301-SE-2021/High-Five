package com.bdpsolutions.highfive.subsystems.image.adapter.provider

import com.bdpsolutions.highfive.subsystems.image.adapter.*
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
abstract class ImageRecyclerViewAdapterProvider {
    @Binds
    abstract fun bindRecyclerView(
        recyclerViewImpl: ImageRecyclerViewAdapterImpl
    ): ImageRecyclerViewAdapter
}