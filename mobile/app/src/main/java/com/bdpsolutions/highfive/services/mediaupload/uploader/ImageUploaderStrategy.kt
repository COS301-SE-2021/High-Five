package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.RepositoryTypes
import com.bdpsolutions.highfive.subsystems.image.model.repository.ImageRepository
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import java.io.File
import javax.inject.Inject

class ImageUploaderStrategy constructor(repoFactory: RepositoryFactory) : IUploaderStrategy(repoFactory) {

    override fun uploadFile(path: String) {
        (repoFactory.createRepository(RepositoryTypes.IMAGE_REPOSITORY) as ImageRepository).storeImage(File(path))
    }
}