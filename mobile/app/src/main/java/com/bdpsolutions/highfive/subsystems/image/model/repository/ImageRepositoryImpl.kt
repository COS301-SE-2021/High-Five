package com.bdpsolutions.highfive.subsystems.image.model.repository

import android.util.Log
import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.image.model.source.ImageDataSource
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import com.bdpsolutions.highfive.utils.Result
import io.reactivex.FlowableEmitter
import io.reactivex.Observer
import java.io.File

class ImageRepositoryImpl(source: ImageDataSource): ImageRepository(source) {
    override fun fetchImages(imageObservable: MutableLiveData<ImageResult>) {
        source.fetchAllImages(imageObservable)
    }

    override fun storeImage(image: File,
                            progressObserver: Observer<Double>,
                            resultObserver: Observer<Result<String>>
    ) {
        source.loadImage(image, progressObserver, resultObserver)
    }

    companion object {
        fun create(source : ImageDataSource): ImageRepositoryImpl {
            return ImageRepositoryImpl(source)
        }
    }
}