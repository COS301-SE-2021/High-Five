package com.bdpsolutions.highfive.helpers

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import org.mockito.Mockito.*

class ViewModelMockRepositoryFactory : ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {

            val mockAPI = mock(APILogin::class.java)

//            `when`(mockAPI.login("hello", "world")).thenReturn(
//                Result.Success(User(0, "xxxx"))
//            )

            return LoginViewModel(
                loginRepository = LoginRepository(
                    loginSource = mockAPI
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