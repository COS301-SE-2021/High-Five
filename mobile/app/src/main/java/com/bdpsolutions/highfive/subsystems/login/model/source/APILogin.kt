package com.bdpsolutions.highfive.subsystems.login.model.source

import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.appauth.Configuration
import net.openid.appauth.*
import android.content.Intent
import net.openid.appauth.AuthorizationService
import androidx.activity.result.ActivityResultLauncher
import net.openid.appauth.AuthorizationException

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class APILogin : LoginDataSource {


    override fun login(resultLauncher: ActivityResultLauncher<Intent>) {

        val mConfiguration = Configuration.getInstance(ContextHolder.appContext!!)

        AuthorizationServiceConfiguration.fetchFromUrl(
            mConfiguration.discoveryUri!!,
            { config: AuthorizationServiceConfiguration?, ex: AuthorizationException? ->
                val authRequestBuilder = AuthorizationRequest.Builder(
                    config!!,
                    mConfiguration.clientId!!,
                    ResponseTypeValues.CODE,
                    mConfiguration.redirectUri
                )
                    .setScope(mConfiguration.scope)
                    .setPrompt("login")
                val authService = AuthorizationService(ContextHolder.appContext!!)
                val authIntent = authService.getAuthorizationRequestIntent(authRequestBuilder.build())
                resultLauncher.launch(authIntent)

            },
            mConfiguration.connectionBuilder
        )
    }

    override fun logout() {
        // TODO: revoke authentication
    }
}