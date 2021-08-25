package com.bdpsolutions.highfive.subsystems.image.model.repository

import android.util.Log
import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.model.source.ImageDataSource
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import java.io.File

class ImageRepositoryImpl(source: ImageDataSource): ImageRepository(source) {
    override fun fetchImages(imageObservable: MutableLiveData<ImageResult>) {
        source.fetchAllImages(imageObservable)
    }

    override fun storeImage(image: File, callback: (() -> Unit)?) {
        source.loadImage(image, callback)
    }

    companion object {
        fun create(source : ImageDataSource): ImageRepositoryImpl {
            return ImageRepositoryImpl(source)
        }
    }
}