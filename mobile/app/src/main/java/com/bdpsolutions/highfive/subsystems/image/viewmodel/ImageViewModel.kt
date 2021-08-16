package com.bdpsolutions.highfive.subsystems.image.viewmodel

import android.R.attr
import androidx.activity.ComponentActivity
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
import android.graphics.ImageDecoder
import android.net.Uri
import android.os.Build
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.ActivityResultLauncher
import androidx.fragment.app.Fragment
import java.io.FileNotFoundException
import java.io.IOException


class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.fetchImages(_imageResult)
    }

    private var galleryResultLauncher: ActivityResultLauncher<Intent>? = null
    private var cameraResultLauncher: ActivityResultLauncher<Intent>? = null

    fun registerFetchFromGallery(activity: Fragment) {
        galleryResultLauncher = activity.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                val selectedImage: Uri = result.data?.data!!
                var bitmap: Bitmap? = null
                try {
                    bitmap = if(Build.VERSION.SDK_INT < 28) {
                        MediaStore.Images.Media.getBitmap(
                            activity.requireActivity().contentResolver,
                            selectedImage
                        )

                    } else {
                        val source = ImageDecoder.createSource(activity.requireActivity().contentResolver, selectedImage)
                        ImageDecoder.decodeBitmap(source)
                    }
                    Log.d("Bitmap", "Bitmap retrieved")
                } catch (e: FileNotFoundException) {
                    // TODO Auto-generated catch block
                    e.printStackTrace()
                } catch (e: IOException) {
                    // TODO Auto-generated catch block
                    e.printStackTrace()
                }
            }
        }
    }

    fun launchGalleryChooser(intent: Intent) {
        galleryResultLauncher?.launch(intent)
    }

    companion object {
        fun create(repo: ImageRepository): ImageViewModel {
            return ImageViewModel(repo)
        }
    }
}