package com.bdpsolutions.highfive.subsystems.image.viewmodel

import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageInfo
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageList
import com.bdpsolutions.highfive.utils.Result

data class ImageResult (
    val success: Result<ImageList>? = null,
    val error: Int? = null
)