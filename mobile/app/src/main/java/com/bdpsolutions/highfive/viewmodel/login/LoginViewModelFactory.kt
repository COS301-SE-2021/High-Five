package com.bdpsolutions.highfive.viewmodel.login

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.bdpsolutions.highfive.models.login.source.APILogin
import com.bdpsolutions.highfive.models.login.LoginRepository

/**
 * ViewModel provider factory to instantiate LoginViewModel.
 * Required given LoginViewModel has a non-empty constructor
 */
class LoginViewModelFactory : ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {
            return LoginViewModel(
                loginRepository = LoginRepository(
                    loginSource = APILogin()
                )
            ) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}