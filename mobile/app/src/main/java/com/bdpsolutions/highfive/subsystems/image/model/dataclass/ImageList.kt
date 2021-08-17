package com.bdpsolutions.highfive.subsystems.image.model.dataclass

import com.google.gson.annotations.SerializedName

data class ImageList(
    @SerializedName("images") val images: List<ImageInfo>?,
)
