package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import com.bdpsolutions.highfive.constants.Endpoints
import retrofit2.Call
import retrofit2.http.*

/**
 * Retrofit interface to fetch an access token from Azure.
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 */
interface AccessTokenEndpoint {

    @FormUrlEncoded
    @POST(Endpoints.AUTH.AUTH_TOKEN)
    fun getAccessToken(
        @Field("client_id") clientId: String,
        @Field("scope") scope: String,
        @Field("code") code: String,
        @Field("redirect_uri") redirectUri: String,
        @Field("grant_type") grantType: String,
        @Field("code_verifier") codeVerifier: String
    ): Call<AccessTokenResponse>

    @FormUrlEncoded
    @POST(Endpoints.AUTH.AUTH_TOKEN)
    fun getNewAccessToken(
        @Field("client_id") clientId: String,
        @Field("grant_type") grantType: String,
        @Field("refresh_token") refreshToken: String,
        @Field("redirect_uri") redirectUri: String
    ): Call<AccessTokenResponse>
}