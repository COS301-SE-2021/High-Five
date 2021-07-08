package com.bdpsolutions.highfive.viewmodel.video

import com.bdpsolutions.highfive.view.views.VideoItemView

data class VideoItemResult(
    val success: VideoItemView? = null,
    val error: Int? = null
)