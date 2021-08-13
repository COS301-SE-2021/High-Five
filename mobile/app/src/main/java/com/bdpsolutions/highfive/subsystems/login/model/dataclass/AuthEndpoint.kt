package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import com.bdpsolutions.highfive.constants.Endpoints
import retrofit2.Call
import retrofit2.http.*

interface AuthEndpoint {

    @FormUrlEncoded
    @POST(Endpoints.AUTH.AUTH_TOKEN)
    fun getAccessToken(
        @Field("client_id") client_id: String,
        @Field("scope") scope: String,
        @Field("code") code: String,
        @Field("redirect_uri") redirect_uri: String,
        @Field("grant_type") grant_type: String,
        @Field("code_verifier") code_verifier: String
    ): Call<AccessToken>
}