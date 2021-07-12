package com.bdpsolutions.highfive.models.video

import android.media.MediaRecorder
import com.bdpsolutions.highfive.models.video.source.VideoDataSource

class VideoDataRepository(val apiVideoSource: VideoDataSource, val dbVideoSource: VideoDataSource) {
}