package com.bdpsolutions.highfive.subsystems.login.model.source

import android.util.Log
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AccessTokenEndpoint
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AccessTokenResponse
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.*
import com.google.gson.GsonBuilder
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class APIRefreshToken : LoginDataSource {
    override fun refreshToken() {
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

        //NOTE: This assumes that the refresh token is in the database
        val db = DatabaseHandler.getDatabase(null).userDao()
        val refreshToken = db.getUser()?.refreshToken!!

        val tokenSource = retrofit.create(AccessTokenEndpoint::class.java)
        val config = AzureConfiguration.getInstance()
        val call = tokenSource.getNewAccessToken(
            clientId = config.clientId,
            redirectUri = config.redirectUri,
            refreshToken = refreshToken,
            grantType = "refresh_token",
        )

        call.enqueue(object : Callback<AccessTokenResponse> {

            override fun onResponse(
                call: Call<AccessTokenResponse>,
                response: Response<AccessTokenResponse>
            ) {

                if (response.isSuccessful) {
                    val accessToken = response.body()!!
                    ConcurrencyExecutor.execute {
                        db.updateToken(
                            auth_token = accessToken.idToken!!,
                            refresh_token = accessToken.refreshToken!!,
                            auth_expires = GetTimestamp(accessToken.tokenExpires),
                            refresh_expires = GetTimestamp(accessToken.refreshExpires),
                        )
                    }

                } else {
                    Log.e("Error", response.message())
                }
            }

            override fun onFailure(call: Call<AccessTokenResponse>, t: Throwable) {
                Log.e("TOKEN", "Failed to log in: ${t.message}")
            }
        })
    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     *
     * The constructor of the parent class is made private to ensure that only this helper
     * object may instantiate the parent class.
     */
    companion object {
        fun create(): APIRefreshToken {
            return APIRefreshToken()
        }
    }
}