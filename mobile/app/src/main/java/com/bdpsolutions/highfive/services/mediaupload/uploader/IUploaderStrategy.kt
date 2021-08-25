package com.bdpsolutions.highfive.services.mediaupload.uploader

interface IUploaderStrategy {
    fun uploadFile(path: String)
}