package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.constants.RepositoryTypes
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import io.reactivex.FlowableEmitter
import io.reactivex.Observer
import java.io.File

class VideoUploaderStrategy(repoFactory: RepositoryFactory) : IUploaderStrategy(repoFactory) {

    override fun uploadFile(path: String,
                            progressObserver: Observer<Double>,
                            resultObserver: Observer<Result<String>>
    ) {
        (repoFactory.createRepository(RepositoryTypes.VIDEO_REPOSITORY) as VideoDataRepository)
            .storeVideo(File(path))
    }
}