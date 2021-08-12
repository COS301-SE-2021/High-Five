package com.bdpsolutions.highfive.utils.factories

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import com.bdpsolutions.highfive.constants.Exceptions.VIEWMODEL_PROVIDER_FACTORY as vmf
import javax.inject.Inject


/**
 * ViewModel provider factory to instantiate various view model classes.
 */
class ViewModelProviderFactory @Inject constructor(): ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {
            return LoginViewModel.create(
                loginRepository = LoginRepository.create(
                    loginSource = APILogin.create()
                )
            ) as T
        }
        else if (modelClass.isAssignableFrom(VideoViewModel::class.java)) {
            return VideoViewModel.create(
                VideoDataRepository.create(APIVideoDataSource.create(), APIVideoDataSource.create())
            ) as T
        }
        throw IllegalArgumentException(vmf.UNKNOWN_VIEWMODEL)
    }
}