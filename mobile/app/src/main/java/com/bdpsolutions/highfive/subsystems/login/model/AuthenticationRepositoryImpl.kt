package com.bdpsolutions.highfive.subsystems.login.model

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.subsystems.login.model.source.LoginDataSource

/**
 * Class that requests authentication and user information from a data source.
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 */
class AuthenticationRepositoryImpl private constructor(private val loginSource: LoginDataSource) : AuthenticationRepository {

    /**
     * Invokes the login data source to log the user in.
     *
     * @param resultLauncher object that captures the result of the authentication.
     */
    override fun login(resultLauncher: ActivityResultLauncher<Intent>) {
        // handle login
        loginSource.login(resultLauncher)
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     */
    companion object {
        fun create(apiLogin: LoginDataSource?): AuthenticationRepositoryImpl {
            return AuthenticationRepositoryImpl(apiLogin!!)
        }
    }
}