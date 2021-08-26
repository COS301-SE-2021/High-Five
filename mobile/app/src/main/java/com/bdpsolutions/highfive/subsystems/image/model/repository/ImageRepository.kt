package com.bdpsolutions.highfive.subsystems.image.model.repository

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.image.model.source.ImageDataSource
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import com.bdpsolutions.highfive.utils.Result
import io.reactivex.FlowableEmitter
import io.reactivex.Observer
import java.io.File

abstract class ImageRepository constructor(protected val source : ImageDataSource) {
    abstract fun fetchImages(imageObservable: MutableLiveData<ImageResult>)

    abstract fun storeImage(image: File,
                            progressObserver: Observer<Double>,
                            resultObserver: Observer<Result<String>>
    )
}