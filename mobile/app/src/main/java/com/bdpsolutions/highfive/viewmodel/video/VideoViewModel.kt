package com.bdpsolutions.highfive.viewmodel.video

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.models.video.VideoDataRepository
import com.bdpsolutions.highfive.utils.Result

class VideoViewModel(private val repo: VideoDataRepository) : ViewModel() {

    private val _videoResult = MutableLiveData<VideoResult>()
    val videoResult: LiveData<VideoResult> = _videoResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.refreshData {
            result -> run {
                when(result) {
                    is Result.Success<*> -> {
                        _videoResult.value = VideoResult(success = result)
                    }
                    is Result.Error -> {
                        _videoResult.value = VideoResult(error = -1)
                    }
                }
            }
        }
    }
}