package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import com.google.gson.annotations.SerializedName

data class VideoList(
    @SerializedName("videos") val videos: List<VideoInfo>?,
)