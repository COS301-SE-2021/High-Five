package com.bdpsolutions.highfive.subsystems.login.viewmodel

import android.app.Activity
import android.content.Intent
import android.util.Log
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import android.util.Patterns
import androidx.activity.ComponentActivity
import androidx.activity.result.ActivityResultLauncher
import androidx.activity.result.contract.ActivityResultContracts
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository

import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.subsystems.login.view.LoggedInUserView
import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.Result
import net.openid.appauth.AuthorizationException
import net.openid.appauth.AuthorizationResponse

class LoginViewModel private constructor(val loginRepository: LoginRepository) : ViewModel() {

    private val _loginForm = MutableLiveData<LoginFormState>()
    val loginFormState: LiveData<LoginFormState> = _loginForm

    private val _loginResult = MutableLiveData<LoginResult>()
    val loginResult: LiveData<LoginResult> = _loginResult

    private var mRegisterLoginResult: ActivityResultLauncher<Intent>? = null

    fun registerLoginResult(activity: ComponentActivity) {
        mRegisterLoginResult = activity.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result ->
            if (result.resultCode == Activity.RESULT_OK) {
                // There are no request codes
                val data: Intent? = result.data
                Log.d("Intent URI", data?.dataString!!)
                val resp = AuthorizationResponse.fromIntent(data!!)
                val ex = AuthorizationException.fromIntent(data)

                run handleResult@ {
                    resp?.let {
                        Log.d("TOKEN", "User successfully logged in: ${it.authorizationCode}")
                        //TODO: Get the user's first name and set the display name to that.
                        _loginResult.value = LoginResult(success = LoggedInUserView(displayName = "Hello"))
                        return@handleResult
                    }

                    ex?.let {
                        Log.e("TOKEN", "Failed to log in: ${it.message}")
                        _loginResult.value = LoginResult(error = R.string.login_failed)
                        return@handleResult
                    }
                }
            }
        }
    }


    fun login() {
        // can be launched in a separate asynchronous job
        loginRepository.login(mRegisterLoginResult!!)
    }

    fun loginDataChanged(username: String, password: String) {
        if (!isUserNameValid(username)) {
            _loginForm.value = LoginFormState(usernameError = R.string.invalid_username)
        } else if (!isPasswordValid(password)) {
            _loginForm.value = LoginFormState(passwordError = R.string.invalid_password)
        } else {
            _loginForm.value = LoginFormState(isDataValid = true)
        }
    }

    // A placeholder username validation check
    private fun isUserNameValid(username: String): Boolean {
        return if (username.contains('@')) {
            Patterns.EMAIL_ADDRESS.matcher(username).matches()
        } else {
            username.isNotBlank()
        }
    }

    // A placeholder password validation check
    private fun isPasswordValid(password: String): Boolean {
        return password.length > 5
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     */
    companion object {
        fun create(loginRepository: LoginRepository?) : LoginViewModel {
            return LoginViewModel(loginRepository!!)
        }
    }
}