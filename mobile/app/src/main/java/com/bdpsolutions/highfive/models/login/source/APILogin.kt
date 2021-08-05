package com.bdpsolutions.highfive.models.login.source

import android.app.Activity
import android.util.Log
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.models.login.model.User
import com.bdpsolutions.highfive.utils.ContextHolder
import org.apache.commons.io.IOUtils
import java.io.IOException
import java.io.InputStreamReader
import java.io.StringWriter
import java.util.stream.Collectors
import com.microsoft.identity.client.exception.MsalException
import com.microsoft.identity.client.AuthenticationCallback
import com.microsoft.identity.client.*
import com.microsoft.identity.client.exception.*

import com.microsoft.identity.client.ISingleAccountPublicClientApplication


import com.microsoft.identity.client.IPublicClientApplication.ISingleAccountApplicationCreatedListener

import com.microsoft.identity.client.PublicClientApplication
import com.microsoft.identity.client.IAccount
import com.microsoft.identity.client.ISingleAccountPublicClientApplication.CurrentAccountCallback
import com.microsoft.identity.client.IAuthenticationResult
import com.microsoft.identity.client.AcquireTokenParameters
import com.microsoft.identity.client.IPublicClientApplication
import com.microsoft.identity.client.IPublicClientApplication.LoadAccountsCallback


/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class APILogin : LoginDataSource {

    private var mSingleAccountApp: ISingleAccountPublicClientApplication? = null
    private val SCOPES = arrayOf("https://graph.microsoft.com/offline_access", "https://graph.microsoft.com/openid")

    override fun login(successCallback: (String) -> Unit, failCallback: (String) -> Unit): Result<User> {
        try {
            // TODO: handle loggedInUser authentication
            val fakeUser = User(0, "xxxx")
            PublicClientApplication.createSingleAccountPublicClientApplication(ContextHolder.appContext!!,
                R.raw.auth_config_single_account,
                object : ISingleAccountApplicationCreatedListener {
                    override fun onCreated(application: ISingleAccountPublicClientApplication) {
                        /**
                         * This test app assumes that the app is only going to support one account.
                         * This requires "account_mode" : "SINGLE" in the config json file.
                         */
                        mSingleAccountApp = application
                        mSingleAccountApp?.signIn(ContextHolder.appContext!!, null, SCOPES, getAuthInteractiveCallback())
                        //loadAccount()
                    }

                    override fun onError(exception: MsalException) {
                        Log.e("ACC", exception.message!!)
                    }
                })

            return Result.Success(fakeUser)
        } catch (e: Throwable) {
            return Result.Error(IOException("Error logging in", e))
        }
    }

    private fun getAuthInteractiveCallback(): AuthenticationCallback {
        return object : AuthenticationCallback {
            override fun onSuccess(authenticationResult: IAuthenticationResult) {
                /* Successfully got a token, use it to call a protected resource - MSGraph */
                Log.d("ACC", "Successfully authenticated")
                Log.d("ACC", authenticationResult.accessToken)

            }

            override fun onError(exception: MsalException) {
                /* Failed to acquireToken */
                Log.d("Jifisis", "Authentication failed: ${exception.localizedMessage}")
            }

            override fun onCancel() {
                /* User canceled the authentication */
                Log.d("ACC", "User cancelled login.")
            }
        }
    }

    override fun logout() {
        // TODO: revoke authentication
    }
}