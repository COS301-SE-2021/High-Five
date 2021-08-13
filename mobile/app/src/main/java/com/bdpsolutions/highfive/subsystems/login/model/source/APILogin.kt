package com.bdpsolutions.highfive.subsystems.login.model.source

import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.constants.Settings
import androidx.activity.result.ActivityResultLauncher
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import com.bdpsolutions.highfive.constants.TodoStatements
import com.bdpsolutions.highfive.utils.factories.authConverterFactory
import net.openid.appauth.*
import net.openid.appauth.connectivity.DefaultConnectionBuilder

/**
 * Class that logs in a user using Azure AD.
 */
class APILogin private constructor(): LoginDataSource {

    override fun login(resultLauncher: ActivityResultLauncher<Intent>) {

        val bundle: Bundle = ContextHolder.appContext!!.packageManager
            .getApplicationInfo(
                ContextHolder.appContext!!.packageName,
                PackageManager.GET_META_DATA
            ).metaData

        // Perform authentication
        AuthorizationServiceConfiguration.fetchFromUrl(
            Uri.parse(authConverterFactory<String>("discovery_uri", bundle)),
            { config: AuthorizationServiceConfiguration?, ex: AuthorizationException? ->
                val authRequestBuilder = AuthorizationRequest.Builder(
                    config!!,
                    authConverterFactory<String>("client_id", bundle)!!,
                    ResponseTypeValues.CODE,
                    Uri.parse(authConverterFactory<String>("redirect_uri", bundle))
                )
                    .setScope(authConverterFactory<String>("authorization_scope", bundle))
                    .setPrompt(Settings.PROMPT)
                val authService = AuthorizationService(ContextHolder.appContext!!)
                val authIntent = authService.getAuthorizationRequestIntent(authRequestBuilder.build())
                resultLauncher.launch(authIntent)

            },
            DefaultConnectionBuilder.INSTANCE
        )
    }

    override fun logout() {
        TODO(TodoStatements.NOT_YET_IMPLEMENTED)
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     */
    companion object {
        fun create(): APILogin {
            return APILogin()
        }
    }
}