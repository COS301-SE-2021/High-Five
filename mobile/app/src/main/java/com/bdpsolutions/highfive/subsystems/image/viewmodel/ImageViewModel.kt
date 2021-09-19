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
import android.provider.OpenableColumns
import androidx.fragment.app.FragmentActivity
import androidx.localbroadcastmanager.content.LocalBroadcastManager


class ImageViewModel private constructor(private val repo: ImageRepository): ViewModel() {
    private val _imageResult = MutableLiveData<ImageResult>()
    val imageResult: LiveData<ImageResult> = _imageResult

    fun fetchImageData() {
        repo.fetchImages(_imageResult)
    }

    private var imageResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchImage(fragment: Fragment) {
        imageResultLauncher = fragment.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result:ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {

                val path = if (result.data != null && result.data!!.data != null) {
                    val selectedImage = result.data?.data!!
                    val cursor: Cursor = fragment.requireActivity().contentResolver.query(selectedImage, arrayOf(
                        OpenableColumns.DISPLAY_NAME), null, null, null)!!
                    val nameIndex: Int =
                        cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                    cursor.moveToFirst()
                    val name: String = cursor.getString(nameIndex)
                    cursor.close()

                    //Writing image to cache
                    val tempImage = File(fragment.requireActivity().cacheDir.absolutePath + "/$name")
                    val inputStream: InputStream =
                        fragment.requireActivity().contentResolver.openInputStream(selectedImage)!!
                    val outputStream = FileOutputStream(tempImage)
                    var read: Int
                    val bufferSize = 8192
                    val buffers = ByteArray(bufferSize)
                    while (inputStream.read(buffers).also { read = it } != -1) {
                        outputStream.write(buffers, 0, read)
                    }
                    tempImage.path
                } else {
                    fragment.requireActivity().cacheDir.absolutePath + "/" + ImageURL.url?.path!!.split("/")[2]
                }

                Log.d("IMAGE NAME", path!!)

                val uploadFileIntent = Intent(fragment.requireActivity(), MediaUploadService::class.java)
                uploadFileIntent.putExtra("media_file", path)
                uploadFileIntent.putExtra("media_type", MediaTypes.IMAGE)
                uploadFileIntent.putExtra("return", "image_result")

                fragment.requireActivity().startService(uploadFileIntent)
            }
        }
    }

    fun registerServiceReceiver(activity: FragmentActivity, receiver: BroadcastReceiver) {
        LocalBroadcastManager.getInstance(activity)
            .registerReceiver(receiver, IntentFilter("image_result"));
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

    fun launchImageChooser(intent: Intent) {
        imageResultLauncher?.launch(intent)
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