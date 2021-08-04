package com.bdpsolutions.highfive.viewmodel.video

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.utils.Result

data class VideoResult(
    val success: Result<List<VideoPreview>>? = null,
    val error: Int? = null
)