package com.bdpsolutions.highfive.data.login.source

import com.bdpsolutions.highfive.data.login.Result
import com.bdpsolutions.highfive.data.login.model.User

/**
 * Interface for the login/logout functionality. Will allow mocking for unit tests.
 */
interface LoginDataSource {
    fun login(username: String, password: String): Result<User>
    fun logout()
}