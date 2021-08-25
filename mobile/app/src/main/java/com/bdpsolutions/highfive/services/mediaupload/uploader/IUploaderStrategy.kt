package com.bdpsolutions.highfive.services.mediaupload.uploader

import com.bdpsolutions.highfive.utils.factories.RepositoryFactory

abstract class IUploaderStrategy constructor(protected val repoFactory: RepositoryFactory) {
    abstract fun uploadFile(path: String, callback: () -> Unit)
}