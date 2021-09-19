package com.bdpsolutions.highfive.utils.factories

import androidx.lifecycle.ViewModelProvider
import dagger.Binds
import dagger.Module
import dagger.hilt.InstallIn
import dagger.hilt.android.components.ActivityComponent

@Module
@InstallIn(ActivityComponent::class)
abstract class ViewModelFactoryProvider {
    @Binds
    abstract fun bindLoginViewModelFactory(
        loginFactoryImpl: ViewModelProviderFactory
    ) : ViewModelProvider.Factory
}