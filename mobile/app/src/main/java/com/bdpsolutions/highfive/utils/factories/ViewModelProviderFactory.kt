package com.bdpsolutions.highfive.utils.factories

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository
import com.bdpsolutions.highfive.subsystems.image.model.source.APIImageDataSource
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageViewModel
import com.bdpsolutions.highfive.subsystems.login.model.AuthenticationRepositoryImpl
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.login.model.source.APIRefreshToken
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepositoryImpl
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.media.viewmodel.MediaViewModel
import com.bdpsolutions.highfive.subsystems.splash.viewmodel.SplashViewModel
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import com.bdpsolutions.highfive.constants.Exceptions.VIEWMODEL_PROVIDER_FACTORY as vmf
import javax.inject.Inject


/**
 * ViewModel provider factory to instantiate various view model classes.
 */
class ViewModelProviderFactory @Inject constructor(): ViewModelProvider.Factory {

    @Inject
    lateinit var repoFactory : RepositoryFactory

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        when {
            modelClass.isAssignableFrom(LoginViewModel::class.java) -> {
                return LoginViewModel.create(
                    authenticationRepository = AuthenticationRepositoryImpl.create(
                        APILogin.create()
                    )
                ) as T
            }
            modelClass.isAssignableFrom(VideoViewModel::class.java) -> {
                return VideoViewModel.create(
                    repoFactory.createRepository(VideoDataRepository::class.java)
                ) as T
            }
            modelClass.isAssignableFrom(SplashViewModel::class.java) -> {
                return SplashViewModel.create(
                    authenticationRepository = AuthenticationRepositoryImpl.create(
                        APIRefreshToken.create()
                    )
                ) as T
            }
            modelClass.isAssignableFrom(MediaViewModel::class.java) -> {
                return MediaViewModel.create() as T
            }
            modelClass.isAssignableFrom(ImageViewModel::class.java) ->
                return ImageViewModel.create(
                    repo = ImageRepository.create(
                        source = APIImageDataSource.create()
                    )
                ) as T
            else -> throw IllegalArgumentException(vmf.UNKNOWN_VIEWMODEL)
        }
    }
}