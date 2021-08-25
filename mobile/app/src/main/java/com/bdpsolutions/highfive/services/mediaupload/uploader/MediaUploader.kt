package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.MediaTypes
import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import java.lang.Exception
import javax.inject.Inject

class MediaUploader {

    private var mediaType: String? = null
    private var uploader: IUploaderStrategy? = null

    fun setMediaType(type: String) : MediaUploader {
        uploader = when(type) {
            MediaTypes.IMAGE -> ImageUploaderStrategy()
            MediaTypes.VIDEO -> VideoUploaderStrategy()
            else -> throw Exception("Unsupported media type: $type")
        }
        return this
    }

    fun upload() {

    }
}