package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import io.reactivex.Observer

abstract class IUploaderStrategy constructor(protected val repoFactory: RepositoryFactory) {
    abstract fun uploadFile(path: String,
                            progressObserver: Observer<Double>,
                            resultObserver: Observer<Result<String>>
    )
}