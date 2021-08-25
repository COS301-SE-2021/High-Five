package com.bdpsolutions.highfive.services.mediaupload.uploader

import android.content.Context
import com.bdpsolutions.highfive.constants.MediaTypes
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import com.bdpsolutions.highfive.utils.factories.RepositoryFactoryProvider
import dagger.hilt.EntryPoints
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

    fun upload(path: String, callback: () -> Unit) {
        uploader?.uploadFile(path, callback)
    }
}