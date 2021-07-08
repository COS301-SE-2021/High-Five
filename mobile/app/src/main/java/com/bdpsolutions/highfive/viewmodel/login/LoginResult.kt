package com.bdpsolutions.highfive.viewmodel.login

import com.bdpsolutions.highfive.view.views.login.LoggedInUserView

/**
 * Authentication result : success (user details) or error message.
 */
data class LoginResult(
    val success: LoggedInUserView? = null,
    val error: Int? = null
)