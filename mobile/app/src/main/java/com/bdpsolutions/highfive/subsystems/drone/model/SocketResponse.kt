package com.bdpsolutions.highfive.subsystems.drone.model

import com.google.gson.annotations.SerializedName

data class SocketResponse(
    @SerializedName("publishLink") var publishLink: String?,
    @SerializedName("streamId") var streamId: String?,
    @SerializedName("status") var status: String,
    @SerializedName("message") var message: String?,
)