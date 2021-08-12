package com.bdpsolutions.highfive.subsystems.login.model

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.User
import com.bdpsolutions.highfive.subsystems.login.model.source.LoginDataSource
import com.bdpsolutions.highfive.utils.Result

/**
 * Class that requests authentication and user information from the remote data source and
 * maintains an in-memory cache of login status and user credentials information.
 */

class LoginRepository private constructor(private val loginSource: LoginDataSource) {

    // in-memory cache of the loggedInUser object
    var user: User? = null
        private set

    val isLoggedIn: Boolean
        get() = user != null

    init {
        // If user credentials will be cached in local storage, it is recommended it be encrypted
        // @see https://developer.android.com/training/articles/keystore
        user = null
    }

    fun logout() {
        user = null
        loginSource.logout()
    }

    fun login(resultLauncher: ActivityResultLauncher<Intent>) {
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
        fun create(loginSource: LoginDataSource?): LoginRepository {
            return LoginRepository(loginSource!!)
        }
    }
}