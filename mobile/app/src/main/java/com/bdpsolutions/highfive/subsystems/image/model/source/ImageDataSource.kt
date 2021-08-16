package com.bdpsolutions.highfive.subsystems.image.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult

interface ImageDataSource {
    fun fetchAllImages(imageObservable: MutableLiveData<ImageResult>)
}