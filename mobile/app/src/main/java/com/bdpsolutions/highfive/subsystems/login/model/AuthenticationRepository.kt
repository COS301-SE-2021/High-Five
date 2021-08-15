package com.bdpsolutions.highfive.subsystems.login.model

import android.content.Intent
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.constants.TodoStatements.IMPLEMENT_IN_CLASS

interface AuthenticationRepository {
    fun login(resultLauncher: ActivityResultLauncher<Intent>)
    fun refreshToken() {
        TODO(IMPLEMENT_IN_CLASS)
    }
}