package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import com.google.gson.annotations.SerializedName

data class VideoUploadResult(
    @SerializedName("success") val success: Boolean?,
    @SerializedName("videoId") val videoId: String?,
)
