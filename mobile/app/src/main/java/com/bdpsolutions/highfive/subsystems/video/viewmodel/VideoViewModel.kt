package com.bdpsolutions.highfive.subsystems.video.viewmodel

import android.app.Activity
import android.content.BroadcastReceiver
import android.content.Intent
import android.content.IntentFilter
import android.database.Cursor
import android.net.Uri
import android.provider.OpenableColumns
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.ActivityResultLauncher
import androidx.activity.result.contract.ActivityResultContracts
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentActivity
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.bdpsolutions.highfive.constants.MediaTypes
import com.bdpsolutions.highfive.services.mediaupload.MediaUploadService
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import java.io.File
import java.io.FileOutputStream
import java.io.InputStream
import java.util.*

class VideoViewModel private constructor(private val repo: VideoDataRepository) : ViewModel() {

    private val _videoResult = MutableLiveData<VideoResult>()
    val videoResult: LiveData<VideoResult> = _videoResult

    fun fetchVideoData() {
        repo.fetchVideoData(_videoResult)
    }

    private var fetchVideoResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    /**
     * Registers the fragment to a result launcher that uploads a chosen video to the backend.
     */
    fun registerFetchVideo(fragment: Fragment) {
        fetchVideoResultLauncher = fragment.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result: ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                val selectedVideo: Uri = result.data?.data!!
                if (fragment.requireActivity().contentResolver != null) {

                    /*
                    A selected video is copied from it's original source to the app's local cache
                    before uploading. This is done because the service that uploads the video does
                    not have permission to read the file unless it's in the cache.
                     */

                    val cursor: Cursor? = fragment.requireActivity().contentResolver.query(selectedVideo, arrayOf(
                        OpenableColumns.DISPLAY_NAME), null, null, null)
                    if (cursor != null) {

                        //Fetching name of video
                        val nameIndex: Int =
                            cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                        cursor.moveToFirst()
                        val name: String = cursor.getString(nameIndex)
                        cursor.close()

                        //Writing video to cache
                        val tempVideo = File(fragment.requireActivity().filesDir.absolutePath + "/$name")
                        val inputStream: InputStream =
                            fragment.requireActivity().contentResolver.openInputStream(selectedVideo)!!
                        val outputStream = FileOutputStream(tempVideo)
                        var read: Int
                        val bufferSize = 4096
                        val buffers = ByteArray(bufferSize)
                        while (inputStream.read(buffers).also { read = it } != -1) {
                            outputStream.write(buffers, 0, read)
                        }

                        inputStream.close()
                        outputStream.close()

                        //Starting the upload service
                        val uploadFileIntent = Intent(fragment.requireActivity(), MediaUploadService::class.java)
                        uploadFileIntent.putExtra("media_file", tempVideo.path)
                        uploadFileIntent.putExtra("media_type", MediaTypes.VIDEO)
                        uploadFileIntent.putExtra("return", "video_result")

                        fragment.requireActivity().startService(uploadFileIntent)
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

    fun registerServiceReceiver(activity: FragmentActivity, receiver: BroadcastReceiver) {
        LocalBroadcastManager.getInstance(activity)
            .registerReceiver(receiver, IntentFilter("video_result"));
    }

    companion object {
        fun create(repo: VideoDataRepository): VideoViewModel {
            return VideoViewModel(repo)
        }
    }
}