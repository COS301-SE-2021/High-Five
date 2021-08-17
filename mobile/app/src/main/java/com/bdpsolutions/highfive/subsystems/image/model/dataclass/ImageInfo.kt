package com.bdpsolutions.highfive.subsystems.image.model.dataclass

import android.net.Uri
import com.google.gson.annotations.SerializedName
import java.util.*

data class ImageInfo(
    @SerializedName("id") val id: String?,
    @SerializedName("name") val name: String?,
    @SerializedName("dateStored") val dateStored: Date?,
    @SerializedName("url") val url: Uri?,
)
