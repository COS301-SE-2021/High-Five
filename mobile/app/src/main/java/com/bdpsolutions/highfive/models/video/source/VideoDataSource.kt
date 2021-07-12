package com.bdpsolutions.highfive.models.video.source

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.utils.Result

interface VideoDataSource {
    fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit)
    fun addPreviewData(vararg data: VideoPreview)
    fun deletePreviewData(id: String)
}