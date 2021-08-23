package com.bdpsolutions.highfive.subsystems.image.model.dataclass

import com.google.gson.annotations.SerializedName

data class ImageUploadResult(
    @SerializedName("success") val success: Boolean?,
    @SerializedName("imageId") val imageId: String?,
)