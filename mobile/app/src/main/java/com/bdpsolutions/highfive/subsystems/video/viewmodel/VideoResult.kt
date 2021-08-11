package com.bdpsolutions.highfive.subsystems.video.viewmodel

import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.Result

data class VideoResult(
    val success: Result<List<VideoPreview>>? = null,
    val error: Int? = null
)