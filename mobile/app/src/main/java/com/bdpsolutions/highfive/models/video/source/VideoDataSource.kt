package com.bdpsolutions.highfive.models.video.source

import com.bdpsolutions.highfive.models.video.model.VideoPreview

interface VideoDataSource {
    fun getVideoPreviewData() : List<VideoPreview>
    fun addPreviewData(vararg data: VideoPreview)
    fun deletePreviewData(id: String)
}