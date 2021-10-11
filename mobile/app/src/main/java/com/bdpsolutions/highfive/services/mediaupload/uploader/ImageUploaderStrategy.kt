package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.RepositoryTypes
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.image.model.repository.ImageRepository
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import io.reactivex.FlowableEmitter
import io.reactivex.Observer
import java.io.File
import javax.inject.Inject

class ImageUploaderStrategy constructor(repoFactory: RepositoryFactory) : IUploaderStrategy(repoFactory) {

    override fun uploadFile(path: String,
                            progressObserver: Observer<Int>,
                            resultObserver: Observer<Result<String>>
    ) {
        (repoFactory.createRepository(RepositoryTypes.IMAGE_REPOSITORY) as ImageRepository)
            .storeImage(File(path), progressObserver, resultObserver)
    }
}