package com.bdpsolutions.highfive.subsystems.drone.model

import com.google.gson.annotations.SerializedName

data class SocketRequest(
    @SerializedName("Authorization") val authorization: String,
    @SerializedName("Request") val request: String,
    @SerializedName("Body") val body: String?
)
