package com.bdpsolutions.highfive.models.video

import android.media.MediaRecorder
import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.models.video.source.VideoDataSource
import com.bdpsolutions.highfive.utils.Result

class VideoDataRepository(private val apiVideoSource: VideoDataSource,
                          private val dbVideoSource: VideoDataSource) {

    fun refreshData(callback: (Result<List<VideoPreview>>) -> Unit) {
        apiVideoSource.getVideoPreviewData(callback)
    }
}