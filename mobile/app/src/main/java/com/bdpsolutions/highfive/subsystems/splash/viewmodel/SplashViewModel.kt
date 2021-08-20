package com.bdpsolutions.highfive.subsystems.splash.viewmodel

import android.app.Activity
import android.content.Intent
import android.util.Log
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.BuildConfig
import com.bdpsolutions.highfive.constants.Settings
import com.bdpsolutions.highfive.subsystems.main.MainActivity
import com.bdpsolutions.highfive.subsystems.login.LoginActivity
import com.bdpsolutions.highfive.subsystems.login.model.AuthenticationRepository
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.DatabaseHandler
import com.bdpsolutions.highfive.utils.VolleyNetworkManager
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking

/**
 * This class runs logic for the SplashActivity to start the main or login activity based
 * on whether the user is logged in or not.
 */
class SplashViewModel private constructor(private val authenticationRepository: AuthenticationRepository): ViewModel() {

    /**
     * Checks if the user has stored credentials. If they do, they get redirected to the main
     * activity, otherwise they are redirected to the login activity
     *
     * @param context Application context passed for the database
     */
    fun redirectActivity(context: Activity) {

        ConcurrencyExecutor.execute {

            //Initialising managers
            VolleyNetworkManager.getInstance(context)
            DatabaseHandler.getDatabase(context)
            ContextHolder.appContext = context

            // check if the user has saved details. If there are no details, redirect to login
            val user = DatabaseHandler.getDatabase(context).userDao().getUser()

            val intent = if (user == null && !(Settings.DEVELOPMENT && BuildConfig.DEBUG)) {
                Intent(context, LoginActivity::class.java)
            } else {

                val intent: Intent?

                run checkExpired@ {
                    Log.d("Check Expires", "Checking if tokens are expired...")
                    //check if refresh token expired and redirect to login if it has
                    if (checkTokenExpired(user?.refreshExpires!!)) {
                        intent = Intent(context, LoginActivity::class.java)
                        return@checkExpired
                    }
                    intent = Intent(context, MainActivity::class.java)
                    //check if access token is expired and request a new one if it has

                    if (checkTokenExpired(user.authExpires!!)) {
                        runBlocking {
                            launch {
                                authenticationRepository.refreshToken()
                            }
                        }
                    }
                }

                intent!!
            }

            // Allows running of a coroutine in the current thread.
            runBlocking {

                /*
                Launches the coroutine. The coroutine suspends the thread by 1 second, allowing
                the splash screen to show before starting the new activity
                 */
                launch {
                    delay(1000L)
                    context.startActivity(intent)
                    (context as Activity).finish() //destroys the splash screen
                }
            }
        }
    }

    private fun checkTokenExpired(tokenTime: Long) : Boolean {
        return (System.currentTimeMillis() / 1000L) > tokenTime
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     */
    companion object {
        fun create(authenticationRepository: AuthenticationRepository?) : SplashViewModel {
            return SplashViewModel(authenticationRepository!!)
        }
    }
}