package com.bdpsolutions.highfive.subsystems.login.viewmodel

import com.bdpsolutions.highfive.subsystems.login.view.LoggedInUserView

/**
 * Authentication result : success (user details) or error message.
 */
data class LoginResult(
    val success: LoggedInUserView? = null,
    val error: Int? = null
)