package com.bdpsolutions.highfive.services.mediaupload.uploader

abstract class MediaUploader {


    protected abstract fun getEndpoint()
    protected abstract fun doUpload()

    abstract fun upload()
}