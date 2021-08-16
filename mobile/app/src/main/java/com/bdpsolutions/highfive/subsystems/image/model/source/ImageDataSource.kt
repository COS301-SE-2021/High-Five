package com.bdpsolutions.highfive.subsystems.image.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import java.io.File

interface ImageDataSource {
    fun fetchAllImages(imageObservable: MutableLiveData<ImageResult>)
    fun loadImage(image: File, callback: (() -> Unit)? = null)
}