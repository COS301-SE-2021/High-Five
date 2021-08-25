package com.bdpsolutions.highfive.subsystems.video.viewmodel

import android.app.Activity
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.database.Cursor
import android.net.Uri
import android.provider.MediaStore
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
import java.util.*

class VideoViewModel private constructor(private val repo: VideoDataRepository) : ViewModel() {

    private val _videoResult = MutableLiveData<VideoResult>()
    val videoResult: LiveData<VideoResult> = _videoResult

    private var cacheValidUntil: Int = 0

    private val bReceiver: BroadcastReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context, intent: Intent) {
            //put here whaterver you want your activity to do with the intent received
        }
    }

    fun fetchVideoData() {
        repo.fetchVideoData(_videoResult)
    }

    private var fetchVideoResultLauncher: ActivityResultLauncher<Intent>? = null
    private var permissionResultLauncher: ActivityResultLauncher<String>? = null

    fun registerFetchVideo(fragment: Fragment) {
        fetchVideoResultLauncher = fragment.registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result: ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                val selectedVideo: Uri = result.data?.data!!
                if (fragment.requireActivity().contentResolver != null) {
                    val cursor: Cursor? = fragment.requireActivity().contentResolver.query(selectedVideo, null, null, null, null)
                    if (cursor != null) {
                        cursor.moveToFirst()
                        val idx: Int = cursor.getColumnIndex(MediaStore.Video.VideoColumns.DATA)
                        val path = cursor.getString(idx)
                        cursor.close()

                        val uploadFileIntent = Intent(fragment.requireActivity(), MediaUploadService::class.java)
                        uploadFileIntent.putExtra("media_file", path)
                        uploadFileIntent.putExtra("media_type", MediaTypes.VIDEO)

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

    fun registerServiceReceiver(activity: FragmentActivity) {
        LocalBroadcastManager.getInstance(activity)
            .registerReceiver(bReceiver, IntentFilter("video_result"));
    }

    companion object {
        fun create(repo: VideoDataRepository): VideoViewModel {
            return VideoViewModel(repo)
        }
    }
}