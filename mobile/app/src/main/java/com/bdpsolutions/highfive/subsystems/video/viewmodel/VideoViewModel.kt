package com.bdpsolutions.highfive.subsystems.video.viewmodel

import android.app.Activity
import android.content.Intent
import android.database.Cursor
import android.graphics.Bitmap
import android.graphics.ImageDecoder
import android.net.Uri
import android.os.Build
import android.provider.MediaStore
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.ActivityResultLauncher
import androidx.activity.result.contract.ActivityResultContracts
import androidx.fragment.app.Fragment
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.services.mediaupload.MediaUploadService
import com.bdpsolutions.highfive.subsystems.video.fragment.VideoFragment
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ContextHolder
import com.bdpsolutions.highfive.utils.ImageURL
import java.io.ByteArrayOutputStream
import java.io.File
import java.io.FileOutputStream
import java.text.SimpleDateFormat
import java.util.*

class VideoViewModel private constructor(private val repo: VideoDataRepository) : ViewModel() {

    private val _videoResult = MutableLiveData<VideoResult>()
    val videoResult: LiveData<VideoResult> = _videoResult

    private var cacheValidUntil: Int = 0

    fun fetchVideoData() {
        repo.fetchVideoData(_videoResult)
    }

    private var fetchVideoResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchVideo(activity: VideoFragment) {
        fetchVideoResultLauncher = activity.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result: ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                activity.showLoader()
                val selectedVideo: Uri = result.data?.data!!
                if (activity.requireActivity().contentResolver != null) {
                    val cursor: Cursor? = activity.requireActivity().contentResolver.query(selectedVideo, null, null, null, null)
                    if (cursor != null) {
                        cursor.moveToFirst()
                        val idx: Int = cursor.getColumnIndex(MediaStore.Video.VideoColumns.DATA)
                        val path = cursor.getString(idx)
                        cursor.close()

                        val uploadFileIntent = Intent(activity.requireContext(), MediaUploadService::class.java)
                        uploadFileIntent.putExtra("media_file", path)

                        activity.requireActivity().startService(uploadFileIntent)

//                        repo.storeVideo(File(path)) {
//                            activity.refresh()
//                        }
                    }
                }
            }
        }
    }

    fun registerPermission(activity: Fragment, callback: ArrayList<() -> Unit>) {
        permissionResultLauncher = activity.registerForActivityResult(
            ActivityResultContracts.RequestPermission()
        ) { result ->
            if (result) {
                callback[0]() //Dirty hack, but it allows us to dynamically set which function to use.
            } else {
                Log.e("Permission", "onActivityResult: PERMISSION DENIED")
            }
        }
    }

    fun launchVideoChooser(intent: Intent) {
        fetchVideoResultLauncher?.launch(intent)
    }

    fun askPermission(permission: String) {
        permissionResultLauncher?.launch(permission)
    }

    companion object {
        fun create(repo: VideoDataRepository): VideoViewModel {
            return VideoViewModel(repo)
        }
    }
}