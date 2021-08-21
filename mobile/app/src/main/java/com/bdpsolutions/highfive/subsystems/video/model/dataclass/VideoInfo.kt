package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import android.net.Uri
import com.google.gson.annotations.SerializedName
import java.util.*

data class VideoInfo(
    @SerializedName("id") val id: String?,
    @SerializedName("name") val name: String?,
    @SerializedName( "dateStored") val dateStored: Date?,
    @SerializedName( "url") val url: Uri?,
    //@SerializedName( "thumbnail") val thumbnail: String?
)