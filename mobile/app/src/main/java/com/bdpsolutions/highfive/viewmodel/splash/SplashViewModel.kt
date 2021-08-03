package com.bdpsolutions.highfive.viewmodel.splash

import android.app.Activity
import android.content.Context
import android.content.Intent
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.view.activities.MainActivity
import com.bdpsolutions.highfive.view.activities.LoginActivity
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.DatabaseHandler
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking

/**
 * This class runs logic for the SplashActivity to start the main or login activity based
 * on whether the user is logged in or not.
 */
class SplashViewModel : ViewModel() {

    /**
     * Checks if the user has stored credentials. If they do, they get redirected to the main
     * activity, otherwise they are redirected to the login activity
     *
     * @param context Application context passed for the database
     */
    fun redirectActivity(context: Context) {

        ConcurrencyExecutor.execute {

            // check if the user has saved details. If there are no details, redirect to login
            val user = DatabaseHandler.getDatabase(context).userDao().getUser()
            val intent = if (user == null) {
                Intent(context, LoginActivity::class.java)
            } else {
                Intent(context, MainActivity::class.java)
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
}