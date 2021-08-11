package com.bdpsolutions.highfive.subsystems.login.model.source

import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.appauth.Configuration
import com.bdpsolutions.highfive.constants.Settings
import androidx.activity.result.ActivityResultLauncher
import android.content.Intent
import net.openid.appauth.*

/**
 * Class that logs in a user using Azure AD.
 */
class APILogin : LoginDataSource {


    override fun login(resultLauncher: ActivityResultLauncher<Intent>) {

        val mConfiguration = Configuration.getInstance(ContextHolder.appContext!!)

        // Perform authentication
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
                    .setPrompt(Settings.PROMPT)
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