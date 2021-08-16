package com.bdpsolutions.highfive.subsystems.image.model

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.model.source.ImageDataSource
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult

class ImageRepository private constructor(private val source : ImageDataSource){
    fun fetchImages(imageObservable: MutableLiveData<ImageResult>) {
        source.fetchAllImages(imageObservable)
    }

    companion object {
        fun create(source : ImageDataSource): ImageRepository {
            return ImageRepository(source)
        }
    }
}