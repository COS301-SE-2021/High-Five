package com.bdpsolutions.highfive.subsystems.video.model.repository

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.video.model.source.VideoDataSource
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult
import com.bdpsolutions.highfive.utils.Result
import io.reactivex.Observer
import java.io.File

abstract class VideoDataRepository (protected val source: VideoDataSource){

    abstract fun fetchVideoData(videoObservable: MutableLiveData<VideoResult>)

    abstract fun storeVideo(image: File,
                            progressObserver: Observer<Int>,
                            resultObserver: Observer<Result<String>>
    )
}