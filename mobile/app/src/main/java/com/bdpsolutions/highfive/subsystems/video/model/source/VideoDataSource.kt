package com.bdpsolutions.highfive.subsystems.video.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult
import java.io.File

interface VideoDataSource {
    fun fetchAllVideos(videoObservable: MutableLiveData<VideoResult>)
    fun loadVideo(image: File, callback: (() -> Unit)? = null)
}