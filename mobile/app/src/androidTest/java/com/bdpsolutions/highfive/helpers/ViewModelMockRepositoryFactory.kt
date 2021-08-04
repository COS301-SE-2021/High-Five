package com.bdpsolutions.highfive.helpers

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.models.login.LoginRepository
import com.bdpsolutions.highfive.models.login.model.User
import com.bdpsolutions.highfive.models.login.source.APILogin
import com.bdpsolutions.highfive.models.video.VideoDataRepository
import com.bdpsolutions.highfive.models.video.source.APIVideoDataSource
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.viewmodel.login.LoginViewModel
import com.bdpsolutions.highfive.viewmodel.video.VideoViewModel
import org.mockito.Mockito.*
import org.mockito.Mockito.`when`

class ViewModelMockRepositoryFactory : ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {

            val mockAPI = mock(APILogin::class.java)

            `when`(mockAPI.login("hello", "world")).thenReturn(
                Result.Success(User(0, "xxxx"))
            )

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