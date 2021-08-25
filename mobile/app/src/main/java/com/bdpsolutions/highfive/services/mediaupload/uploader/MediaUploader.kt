package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import javax.inject.Inject

class MediaUploader {

    private var mediaType: String? = null
    private var repository:

    @Inject
    lateinit var repoFactory: RepositoryFactory

    fun setMediaType(type: String) : MediaUploader {
        mediaType = type
        return this
    }

    fun upload() {

    }
}