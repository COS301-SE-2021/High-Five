package com.bdpsolutions.highfive.subsystems.video.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult

interface VideoDataSource {
    fun fetchAllVideos(videoObservable: MutableLiveData<VideoResult>)
}