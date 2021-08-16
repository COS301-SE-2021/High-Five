package com.bdpsolutions.highfive.subsystems.image.viewmodel

import androidx.activity.result.contract.ActivityResultContracts
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository
import android.provider.MediaStore

import android.graphics.Bitmap

import android.R.attr.data

import android.app.Activity
import android.content.Intent
import android.database.Cursor
import android.graphics.ImageDecoder
import android.net.Uri
import android.os.Build
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.ActivityResultLauncher
import androidx.fragment.app.Fragment
import java.io.File
import java.io.FileNotFoundException
import java.io.IOException
import androidx.activity.result.ActivityResultCallback
import androidx.activity.result.contract.ActivityResultContracts.RequestPermission


class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.fetchImages(_imageResult)
    }

    private var galleryResultLauncher: ActivityResultLauncher<Intent>? = null
    private var cameraResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchFromGallery(activity: Fragment) {
        galleryResultLauncher = activity.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                val selectedImage: Uri = result.data?.data!!
                if (activity.requireActivity().contentResolver != null) {
                    val cursor: Cursor? = activity.requireActivity().contentResolver.query(selectedImage, null, null, null, null)
                    if (cursor != null) {
                        cursor.moveToFirst()
                        val idx: Int = cursor.getColumnIndex(MediaStore.Images.ImageColumns.DATA)
                        val path = cursor.getString(idx)
                        repo.storeImage(File(path))
                        cursor.close()
                    }
                }
            }
        }
    }

    fun registerAccessStoragePermission(activity: Fragment, callback: () -> Unit) {
        permissionResultLauncher = activity.registerForActivityResult(
            RequestPermission()
        ) { result ->
            if (result) {
                callback()
            } else {
                Log.e("Permission", "onActivityResult: PERMISSION DENIED")
            }
        }
    }

    fun launchGalleryChooser(intent: Intent) {
        galleryResultLauncher?.launch(intent)
    }

    fun askPermission(permission: String) {
        permissionResultLauncher?.launch(permission)
    }

    companion object {
        fun create(repo: ImageRepository): ImageViewModel {
            return ImageViewModel(repo)
        }
    }
}