package com.bdpsolutions.highfive.subsystems.video.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult
import com.bdpsolutions.highfive.utils.Result
import io.reactivex.Observer
import java.io.File

interface VideoDataSource {
    fun fetchAllVideos(videoObservable: MutableLiveData<VideoResult>)
    fun loadVideo(image: File,
                  progressObserver: Observer<Int>,
                  resultObserver: Observer<Result<String>>)
}