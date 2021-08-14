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
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AccessTokenResponse
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AccessTokenEndpoint
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AuthToken
import com.bdpsolutions.highfive.subsystems.login.view.LoggedInUserView
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.*
import com.google.gson.GsonBuilder
import net.openid.appauth.AuthorizationException
import net.openid.appauth.AuthorizationResponse
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


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
                val resp = AuthorizationResponse.fromIntent(data!!)
                val ex = AuthorizationException.fromIntent(data)

                run handleResult@ {
                    resp?.let {
                        // create a deserializer to create the VideoPreview objects
                        val gson = GsonBuilder()
                            .registerTypeHierarchyAdapter(
                                VideoPreview::class.java,
                                RetrofitDeserializers.AccessTokenDeserializer
                            ).create()

                        // create Retrofit object and fetch data
                        val retrofit = Retrofit.Builder()
                            .baseUrl(Endpoints.AUTH.BASE_URL)
                            .addConverterFactory(GsonConverterFactory.create(gson))
                            .build()

                        val tokenSource = retrofit.create(AccessTokenEndpoint::class.java)
                        val config = AzureConfiguration.getInstance()
                        val call = tokenSource.getAccessToken(
                            client_id = config.clientId,
                            scope = config.scope,
                            redirect_uri = config.redirectUri,
                            code = it.authorizationCode!!,
                            grant_type = "authorization_code",
                            code_verifier = config.codeVerifier
                        )

                        // Enqueue callback object that will call the callback function passed to this function
                        call.enqueue(object : Callback<AccessTokenResponse> {

                            override fun onResponse(
                                call: Call<AccessTokenResponse>,
                                response: Response<AccessTokenResponse>
                            ) {

                                if (response.isSuccessful) {
                                    val accessToken = response.body()!!

                                    ConcurrencyExecutor.execute {

                                        val db = DatabaseHandler.getDatabase(null)

                                        db.userDao().addUser(
                                            AuthToken(
                                                uid = 1,
                                                authToken = accessToken.idToken,
                                                refreshToken = accessToken.refreshToken,
                                                authExpires = accessToken.tokenExpires,
                                                refreshExpires = accessToken.refreshExpires
                                            )
                                        )
                                    }

                                    _loginResult.value = LoginResult(
                                        success = LoggedInUserView(
                                            displayName = JWTDecoder.getFirstName(
                                                accessToken.idToken!!
                                            )!!
                                        )
                                    )
                                } else {
                                    Log.e("Error", response.message())
                                }
                            }

                            override fun onFailure(call: Call<AccessTokenResponse>, t: Throwable) {
                                Log.e("TOKEN", "Failed to log in: ${t.message}")
                                _loginResult.value = LoginResult(error = R.string.login_failed)
                            }
                        })
                        return@handleResult
                    }

                    ex?.let {
                        Log.e("TOKEN", "Failed to log in: ${it.message}")
                        _loginResult.value = LoginResult(error = R.string.login_failed)
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