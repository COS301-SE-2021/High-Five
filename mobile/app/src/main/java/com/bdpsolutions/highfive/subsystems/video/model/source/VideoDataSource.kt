package com.bdpsolutions.highfive.subsystems.video.model.source

import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.Result

interface VideoDataSource {
    fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit)
    fun addPreviewData(vararg data: VideoPreview) {
        // Default implementation. This function is not needed by all child classes,
        // So the implementation is blank so that the child does not have to override
        // This function
    }
    fun deletePreviewData(id: String) {
        // Default implementation. This function is not needed by all child classes,
        // So the implementation is blank so that the child does not have to override
        // This function
    }
}