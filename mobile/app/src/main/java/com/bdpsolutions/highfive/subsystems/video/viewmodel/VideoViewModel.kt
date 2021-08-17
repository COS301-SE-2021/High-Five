package com.bdpsolutions.highfive.subsystems.video.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.utils.Result

class VideoViewModel private constructor(private val repo: VideoDataRepository) : ViewModel() {

    private val _videoResult = MutableLiveData<VideoResult>()
    val videoResult: LiveData<VideoResult> = _videoResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.fetchVideoData(_videoResult)
    }

    companion object {
        fun create(repo: VideoDataRepository): VideoViewModel {
            return VideoViewModel(repo)
        }
    }
}