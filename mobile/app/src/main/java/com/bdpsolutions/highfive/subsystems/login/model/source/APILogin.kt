package com.bdpsolutions.highfive.subsystems.login.model.source

import android.util.Log
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.User
import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.appauth.AuthStateManager
import com.bdpsolutions.highfive.utils.appauth.Configuration
import net.openid.appauth.*
import net.openid.appauth.AuthorizationService.RegistrationResponseCallback
import net.openid.appauth.AuthorizationServiceConfiguration.RetrieveConfigurationCallback
import net.openid.appauth.browser.AnyBrowserMatcher
import java.io.IOException
import java.lang.Exception
import java.util.concurrent.atomic.AtomicReference

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class APILogin : LoginDataSource {

    private val mClientId = AtomicReference<String>()
    private val mAuthRequest = AtomicReference<AuthorizationRequest>()

    override fun login(successCallback: (String) -> Unit, failCallback: (String) -> Unit): Result<User> {

//        val mAuthStateManager = AuthStateManager.getInstance(ContextHolder.appContext!!)
//        val mConfiguration = Configuration.getInstance(ContextHolder.appContext!!)
//        val authService = createAuthorizationService(mConfiguration)
//
//        AuthorizationServiceConfiguration.fetchFromUrl(
//            mConfiguration.discoveryUri!!,
//            { config: AuthorizationServiceConfiguration?, ex: AuthorizationException? ->
//                this.handleConfigurationRetrievalResult(
//                    config,
//                    authService,
//                    mConfiguration,
//                    mAuthStateManager,
//                    ex!!
//                )
//            },
//            mConfiguration.connectionBuilder
//        )

        try {
            // TODO: handle loggedInUser authentication
            val fakeUser = User(0, "xxxx")

            return Result.Success(fakeUser)
        } catch (e: Throwable) {
            return Result.Error(IOException("Error logging in", e))
        }
    }

    override fun logout() {
        // TODO: revoke authentication
    }

    private fun createAuthorizationService(mConfiguration: Configuration): AuthorizationService {
        val builder = AppAuthConfiguration.Builder()
        builder.setBrowserMatcher(AnyBrowserMatcher.INSTANCE)
        builder.setConnectionBuilder(mConfiguration.connectionBuilder)
        return AuthorizationService(ContextHolder.appContext!!, builder.build())
    }

    private fun handleConfigurationRetrievalResult(
        authConfig: AuthorizationServiceConfiguration?,
        authService: AuthorizationService,
        config: Configuration,
        authState: AuthStateManager,
        ex: AuthorizationException
    ) {
        if (authConfig == null) {
            Log.i("LoginActivity", "Failed to retrieve discovery document", ex)
            throw Exception("Failed to retrieve discovery document: ${ex.message}")
        }
        Log.i("LoginActivity", "Discovery document retrieved")
        authState.replace(AuthState(authConfig))
        val registrationRequest = RegistrationRequest.Builder(
            authState.current.authorizationServiceConfiguration!!,
            listOf(config.redirectUri)
        )
            .setTokenEndpointAuthenticationMethod(ClientSecretBasic.NAME)
            .build()

        authService.performRegistrationRequest(
            registrationRequest
        ) { response: RegistrationResponse?, ex2: AuthorizationException? ->
            this.handleRegistrationResponse(
                response,
                authState,
                config,
                ex2!!
            )
        }
    }

    private fun handleRegistrationResponse(
        response: RegistrationResponse?,
        authState: AuthStateManager,
        config: Configuration,
        ex: AuthorizationException
    ) {
        authState.updateAfterRegistration(response, ex)
        if (response == null) {
            Log.i("LoginActivity", "Failed to dynamically register client", ex)
            throw Exception("Failed to register client: ${ex.message}")
        }
        mClientId.set(response.clientId)
        val authRequestBuilder = AuthorizationRequest.Builder(
            authState.current.authorizationServiceConfiguration!!,
            mClientId.get(),
            ResponseTypeValues.CODE,
            config.redirectUri
        )
            .setScope(config.scope)
            .setPrompt("login")
        mAuthRequest.set(authRequestBuilder.build())
    }
}