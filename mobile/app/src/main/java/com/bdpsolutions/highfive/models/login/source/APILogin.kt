package com.bdpsolutions.highfive.models.login.source

import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.models.login.model.User
import java.io.IOException

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class APILogin : LoginDataSource {

    override fun login(username: String, password: String): Result<User> {
        try {
            // TODO: handle loggedInUser authentication
            val fakeUser = User(0, "xxxx")
            return Result.Success(fakeUser)
        } catch (e: Throwable) {
            return Result.Error(IOException("Error logging in", e))
        }
    }

    override fun logout() {
        // TODO: revoke authentication
    }
}