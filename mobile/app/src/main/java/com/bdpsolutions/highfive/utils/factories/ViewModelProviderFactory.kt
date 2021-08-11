package com.bdpsolutions.highfive.utils.factories

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import javax.inject.Inject


/**
 * ViewModel provider factory to instantiate various view model classes.
 */
class ViewModelProviderFactory @Inject constructor(): ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {
            return LoginViewModel(
                loginRepository = LoginRepository(
                    loginSource = APILogin()
                )
            ) as T
        }
        else if (modelClass.isAssignableFrom(VideoViewModel::class.java)) {
            return VideoViewModel(
                VideoDataRepository(APIVideoDataSource(), APIVideoDataSource())
            ) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}