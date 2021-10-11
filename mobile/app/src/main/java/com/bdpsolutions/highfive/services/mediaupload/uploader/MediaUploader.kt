package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.MediaTypes
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import io.reactivex.FlowableEmitter
import io.reactivex.Observer
import java.lang.Exception

class MediaUploader(private var repositoryFactory: RepositoryFactory) {

    private var uploader: IUploaderStrategy? = null

    fun setMediaType(type: String) : MediaUploader {
        uploader = when(type) {
            MediaTypes.IMAGE -> ImageUploaderStrategy(repositoryFactory)
            MediaTypes.VIDEO -> VideoUploaderStrategy(repositoryFactory)
            else -> throw Exception("Unsupported media type: $type")
        }
        return this
    }

    fun upload(path: String,
               progressObserver: Observer<Int>,
               resultObserver: Observer<Result<String>>
    ) {
        uploader?.uploadFile(path, progressObserver, resultObserver)
    }
}