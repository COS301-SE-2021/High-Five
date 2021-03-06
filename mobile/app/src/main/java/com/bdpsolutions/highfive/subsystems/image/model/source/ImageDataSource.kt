package com.bdpsolutions.highfive.subsystems.image.model.source

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import com.bdpsolutions.highfive.utils.Result
import io.reactivex.Observer
import java.io.File

interface ImageDataSource {
    fun fetchAllImages(imageObservable: MutableLiveData<ImageResult>)
    fun loadImage(image: File,
                  progressObserver: Observer<Int>,
                  resultObserver: Observer<Result<String>>
    )
}