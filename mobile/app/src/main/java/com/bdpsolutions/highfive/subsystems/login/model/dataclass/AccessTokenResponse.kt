package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import com.google.gson.annotations.SerializedName

/**
 * Access token response Retrofit uses to store the response data in.
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 */
data class AccessTokenResponse(
    @SerializedName("scope") val scope: String?,
    @SerializedName("id_token_expires_in") val tokenExpires: Int?,
    @SerializedName("refresh_token_expires_in") val refreshExpires: Int?,
    @SerializedName("id_token") val idToken: String?,
    @SerializedName("refresh_token") val refreshToken: String?
)
