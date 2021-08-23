package com.bdpsolutions.highfive.subsystems.image.view

import android.net.Uri

data class ImageItemView(
    val imageName: String,
    val imageId: String,
    val imageDate: String,
    val imageUrl: Uri
)