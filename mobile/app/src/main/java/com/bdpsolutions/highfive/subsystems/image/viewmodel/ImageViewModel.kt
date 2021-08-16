package com.bdpsolutions.highfive.subsystems.image.viewmodel

import androidx.activity.result.contract.ActivityResultContracts
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.image.model.ImageRepository
import android.provider.MediaStore

import android.graphics.Bitmap

import android.app.Activity
import android.content.Intent
import android.database.Cursor
import android.net.Uri
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.ActivityResultLauncher
import androidx.fragment.app.Fragment
import androidx.activity.result.contract.ActivityResultContracts.RequestPermission
import java.text.SimpleDateFormat
import java.util.*
import android.graphics.Bitmap.CompressFormat
import android.graphics.ImageFormat
import com.bdpsolutions.highfive.subsystems.image.ImageFragment

import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import java.io.*


class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    private var cacheValidUntil: Int = 0

    fun fetchImageData() {
        repo.fetchImages(_imageResult)
    }

    private var galleryResultLauncher: ActivityResultLauncher<Intent>? = null
    private var cameraResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchFromGallery(activity: ImageFragment) {
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
                        activity.refresh()
                    }
                }
            }
        }
    }

    fun registerFetchFromCamera(activity: ImageFragment) {
        cameraResultLauncher = activity.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                ConcurrencyExecutor.execute {
                    val selectedImage: Bitmap = result.data?.extras?.get("data") as Bitmap
                    val outputDir: File = activity.requireContext().cacheDir
                    val formatter = SimpleDateFormat("yyyy_MM_dd-HH:mm", Locale.getDefault())
                    val date = Date()

                    val bos = ByteArrayOutputStream()
                    selectedImage.compress(CompressFormat.JPEG, 80 /*ignored for PNG*/, bos)
                    val bitmapdata: ByteArray = bos.toByteArray()

                    val outputFile =
                        File.createTempFile("IMG_${formatter.format(date)}", ".jpg", outputDir)
                    val fos = FileOutputStream(outputFile)
                    fos.write(bitmapdata)
                    fos.flush()
                    fos.close()

                    repo.storeImage(outputFile)

                    activity.refresh()
                }
            }
        }
    }

    fun registerPermission(activity: Fragment, callback: ArrayList<() -> Unit>) {
        permissionResultLauncher = activity.registerForActivityResult(
            RequestPermission()
        ) { result ->
            if (result) {
                callback[0]() //Dirty hack, but it allows us to dynamically set which function to use.
            } else {
                Log.e("Permission", "onActivityResult: PERMISSION DENIED")
            }
        }
    }

    fun launchGalleryChooser(intent: Intent) {
        galleryResultLauncher?.launch(intent)
    }

    fun launchCamera(intent: Intent) {
        cameraResultLauncher?.launch(intent)
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