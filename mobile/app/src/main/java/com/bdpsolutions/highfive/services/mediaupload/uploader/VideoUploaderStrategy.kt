package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.RepositoryTypes
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import java.io.File

class VideoUploaderStrategy(repoFactory: RepositoryFactory) : IUploaderStrategy(repoFactory) {

    override fun uploadFile(path: String, callback: () -> Unit) {
        (repoFactory.createRepository(RepositoryTypes.VIDEO_REPOSITORY) as VideoDataRepository)
            .storeVideo(File(path), callback)
    }
}