package com.bdpsolutions.highfive.subsystems.video.viewmodel

import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoList
import com.bdpsolutions.highfive.utils.Result

data class VideoResult(
    val success: Result<VideoList>? = null,
    val error: Int? = null
)