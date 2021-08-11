package com.bdpsolutions.highfive.subsystems.login.model.source

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.User

/**
 * Interface for the login/logout functionality. Will allow mocking for unit tests.
 */
interface LoginDataSource {
    fun login(resultLauncher: ActivityResultLauncher<Intent>)
    fun logout()
}