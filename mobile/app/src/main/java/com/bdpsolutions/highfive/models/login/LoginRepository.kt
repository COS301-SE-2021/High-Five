package com.bdpsolutions.highfive.models.login

import com.bdpsolutions.highfive.models.login.model.User
import com.bdpsolutions.highfive.models.login.source.LoginDataSource
import com.bdpsolutions.highfive.utils.Result

/**
 * Class that requests authentication and user information from the remote data source and
 * maintains an in-memory cache of login status and user credentials information.
 */

class LoginRepository(private val loginSource: LoginDataSource) {

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

    fun login(successCallback: (String) -> Unit, failCallback: (String) -> Unit): Result<User> {
        // handle login
        val result = loginSource.login(successCallback, failCallback)

        if (result is Result.Success) {
            setLoggedInUser(result.data)
        }

        return result
    }

    private fun setLoggedInUser(loggedInUser: User) {
        this.user = loggedInUser
        // If user credentials will be cached in local storage, it is recommended it be encrypted
        // @see https://developer.android.com/training/articles/keystore
    }
}