package com.bdpsolutions.highfive.subsystems.image.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository

class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.fetchImages(_imageResult)
    }

    companion object {
        fun create(repo: ImageRepository): ImageViewModel {
            return ImageViewModel(repo)
        }
    }
}