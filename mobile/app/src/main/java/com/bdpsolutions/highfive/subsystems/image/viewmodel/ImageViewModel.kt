package com.bdpsolutions.highfive.subsystems.image.viewmodel

import androidx.activity.result.contract.ActivityResultContracts
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.image.model.repository.ImageRepository
import android.provider.MediaStore

import android.graphics.Bitmap

import android.app.Activity
import android.content.Context
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
import android.graphics.ImageDecoder
import android.os.Build
import com.bdpsolutions.highfive.constants.MediaTypes
import com.bdpsolutions.highfive.services.mediaupload.MediaUploadService

import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.ImageURL
import java.io.*
import android.content.BroadcastReceiver
import android.content.IntentFilter
import androidx.fragment.app.FragmentActivity
import androidx.localbroadcastmanager.content.LocalBroadcastManager


class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    private var cacheValidUntil: Int = 0

    private val bReceiver: BroadcastReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context, intent: Intent) {
            //put here whaterver you want your activity to do with the intent received
        }
    }

    fun fetchImageData() {
        repo.fetchImages(_imageResult)
    }

    private var galleryResultLauncher: ActivityResultLauncher<Intent>? = null
    private var cameraResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchFromGallery(fragment: Fragment) {
        galleryResultLauncher = fragment.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                val selectedImage: Uri = result.data?.data!!
                if (fragment.requireActivity().contentResolver != null) {
                    val cursor: Cursor? = fragment.requireActivity().contentResolver.query(selectedImage, null, null, null, null)
                    if (cursor != null) {
                        cursor.moveToFirst()
                        val idx: Int = cursor.getColumnIndex(MediaStore.Images.ImageColumns.DATA)
                        val path = cursor.getString(idx)
                        cursor.close()

                        val uploadFileIntent = Intent(fragment.requireActivity(), MediaUploadService::class.java)
                        uploadFileIntent.putExtra("media_file", path)
                        uploadFileIntent.putExtra("media_type", MediaTypes.IMAGE)
                        uploadFileIntent.putExtra("return", "images")

                        fragment.requireActivity().startService(uploadFileIntent)
                    }
                }
            }
        }
    }

    fun registerServiceReceiver(activity: FragmentActivity) {
        LocalBroadcastManager.getInstance(activity)
            .registerReceiver(bReceiver, IntentFilter("image_result"));
    }

    fun registerFetchFromCamera(fragment: Fragment) {
        cameraResultLauncher = fragment.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                ConcurrencyExecutor.execute {
                    val selectedImage: Bitmap = if(Build.VERSION.SDK_INT < 28) {
                        MediaStore.Images.Media.getBitmap(
                            ContextHolder.appContext!!.contentResolver,
                            ImageURL.url
                        )
                    } else {
                        val source = ImageDecoder.createSource(ContextHolder.appContext!!.contentResolver, ImageURL.url!!)
                        ImageDecoder.decodeBitmap(source)
                    }
                    val outputDir: File = fragment.requireContext().cacheDir
                    val formatter = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
                    val date = Calendar.getInstance().time

                    Log.d("Img dims", "W:${selectedImage.width} H:${selectedImage.height}")


                    val bos = ByteArrayOutputStream()
                    selectedImage.compress(CompressFormat.JPEG, 80 /*ignored for PNG*/, bos)
                    val bitmapdata: ByteArray = bos.toByteArray()

                    val formatDate = formatter.format(date)

                    val outputFile = File(outputDir, "IMG_$formatDate.jpg")
                    val fos = FileOutputStream(outputFile)
                    fos.write(bitmapdata)
                    fos.flush()
                    fos.close()

                    val uploadFileIntent = Intent(fragment.requireActivity(), MediaUploadService::class.java)
                    uploadFileIntent.putExtra("media_file", outputFile.absolutePath)
                    uploadFileIntent.putExtra("media_type", MediaTypes.IMAGE)

                    fragment.requireActivity().startService(uploadFileIntent)

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