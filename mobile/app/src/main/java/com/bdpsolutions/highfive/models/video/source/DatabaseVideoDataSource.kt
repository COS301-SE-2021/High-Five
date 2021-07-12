package com.bdpsolutions.highfive.models.video.source

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.utils.Result

class DatabaseVideoDataSource : VideoDataSource {
    override fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit) {
        TODO("Not yet implemented")
    }

    override fun addPreviewData(vararg data: VideoPreview) {
        TODO("Not yet implemented")
    }

    override fun deletePreviewData(id: String) {
        TODO("Not yet implemented")
    }
}