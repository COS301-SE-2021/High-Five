package com.bdpsolutions.highfive.subsystems.login

import android.app.Activity
import android.content.Intent
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.annotation.StringRes
import androidx.appcompat.app.AppCompatActivity
import android.text.Editable
import android.text.TextWatcher
import android.view.View
import android.view.Window
import android.widget.EditText
import android.widget.Toast

import com.bdpsolutions.highfive.subsystems.login.view.LoggedInUserView
import com.bdpsolutions.highfive.databinding.ActivityLoginBinding
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.main.MainActivity
import com.bdpsolutions.highfive.utils.ContextHolder
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject
import net.openid.appauth.AuthorizationException

import net.openid.appauth.AuthorizationResponse




@AndroidEntryPoint
class LoginActivity : AppCompatActivity() {

    private lateinit var loginViewModel: LoginViewModel
    private lateinit var binding: ActivityLoginBinding
    @Inject
    lateinit var factory: ViewModelProvider.Factory

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        requestWindowFeature(Window.FEATURE_NO_TITLE)
        ContextHolder.appContext = this

        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val login = binding.login
        val loading = binding.loading

        loginViewModel = ViewModelProvider(this, factory)
            .get(LoginViewModel::class.java)

        loginViewModel.registerLoginResult(this)

        loginViewModel.resumeSession()

        loginViewModel.loginResult.observe(this@LoginActivity, Observer {
            val loginResult = it ?: return@Observer

            loading.visibility = View.GONE
            if (loginResult.error != null) {
                showLoginFailed(loginResult.error)
            }
            if (loginResult.success != null) {
                updateUiWithUser(loginResult.success)
                val intent = Intent(this, MainActivity::class.java)
                startActivity(intent)
            }
            setResult(Activity.RESULT_OK)

            //Complete and destroy login activity once successful
            finish()
        })
        login.setOnClickListener {
            loading.visibility = View.VISIBLE
            loginViewModel.login()
        }
    }

    private fun updateUiWithUser(model: LoggedInUserView) {
        val welcome = getString(R.string.welcome)
        val displayName = model.displayName
        // TODO : initiate successful logged in experience
        Toast.makeText(
            applicationContext,
            "$welcome $displayName",
            Toast.LENGTH_LONG
        ).show()
    }

    private fun showLoginFailed(@StringRes errorString: Int) {
        Toast.makeText(applicationContext, errorString, Toast.LENGTH_SHORT).show()
    }
}