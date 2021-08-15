package com.bdpsolutions.highfive.subsystems.login.model.source

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.constants.TodoStatements.IMPLEMENT_IN_CLASS

/**
 * Interface for the login/logout functionality. Will allow mocking for unit tests.
 */
interface LoginDataSource {

    /**
     * Logs in a user.
     *
     * @param resultLauncher launcher that runs once user is logged in
     */
    fun login(resultLauncher: ActivityResultLauncher<Intent>) {
        TODO(IMPLEMENT_IN_CLASS)
    }

    /**
     * Logs out a user.
     */
    fun logout() {
        TODO(IMPLEMENT_IN_CLASS)
    }

    fun refreshToken() {
        TODO(IMPLEMENT_IN_CLASS)
    }
}