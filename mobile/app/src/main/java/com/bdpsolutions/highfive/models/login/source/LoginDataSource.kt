package com.bdpsolutions.highfive.models.login.source

import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.models.login.model.User

/**
 * Interface for the login/logout functionality. Will allow mocking for unit tests.
 */
interface LoginDataSource {
    fun login(successCallback: (String) -> Unit, failCallback: (String) -> Unit): Result<User>
    fun logout()
}