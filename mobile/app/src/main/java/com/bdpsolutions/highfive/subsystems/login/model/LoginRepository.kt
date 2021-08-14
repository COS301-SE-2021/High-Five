package com.bdpsolutions.highfive.subsystems.login.model

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.BuildConfig
import com.bdpsolutions.highfive.constants.Settings
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AuthToken
import com.bdpsolutions.highfive.subsystems.login.model.source.LoginDataSource

/**
 * Class that requests authentication and user information from the remote data source and
 * maintains an in-memory cache of login status and user credentials information.
 */

class LoginRepository private constructor(private val apiLogin: LoginDataSource) {

    // in-memory cache of the loggedInUser object
    var authToken: AuthToken? = null
        private set

    val isLoggedIn: Boolean
        get() = authToken != null

    init {
        // If user credentials will be cached in local storage, it is recommended it be encrypted
        // @see https://developer.android.com/training/articles/keystore
        authToken = null
    }

    fun logout() {
        authToken = null
        apiLogin.logout()
    }

    fun login(resultLauncher: ActivityResultLauncher<Intent>) {
        // handle login
        apiLogin.login(resultLauncher)
    }

    fun resumeSession(callback: () -> Unit) {

        //login regardless of whether an auth token is set or not.
        if (Settings.DEVELOPMENT && BuildConfig.DEBUG) {
            callback()
            return
        }
        //TODO(TodoStatements.CHECK_DB_FOR_TOKEN)
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     */
    companion object {
        fun create(apiLogin: LoginDataSource?): LoginRepository {
            return LoginRepository(apiLogin!!)
        }
    }
}