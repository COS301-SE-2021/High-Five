


package com.bdpsolutions.highfive.subsystems.drone

import android.content.Context
import android.util.Log
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ToastUtils.setResultToToast
import dji.sdk.camera.VideoFeeder
import dji.sdk.camera.VideoFeeder.VideoDataListener
import dji.sdk.camera.VideoFeeder.VideoFeed
import dji.sdk.sdkmanager.LiveStreamManager
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.LiveStreamManager.OnLiveChangeListener
import java.net.URI
import android.content.DialogInterface
import androidx.appcompat.app.AlertDialog
import com.bdpsolutions.highfive.subsystems.main.HighFiveApplication
import com.bdpsolutions.highfive.utils.ContextHolder


class DjiStreamer(){
    private val l = DJISDKManager.getInstance().liveStreamManager
    fun DjiStreamer(){}
    var listener = LiveStreamManager.OnLiveChangeListener {  }

    private fun StartStreaming() {

        if (DJISDKManager.getInstance().liveStreamManager.isStreaming) {
            setResultToToast("already started the Stream!")
            return
        }

        DJISDKManager.getInstance().liveStreamManager.liveUrl = "rtmp://highfiveanalysis.ddns.net/55799ed725ac42bcbb1925c715380541/070482602661397257359202"
        val result = DJISDKManager.getInstance().liveStreamManager.startStream()
        DJISDKManager.getInstance().liveStreamManager.setStartTime()
    }

    fun setupLiveStream() {
        initListener()
        DJISDKManager.getInstance().liveStreamManager.registerListener(listener)
        DJISDKManager.getInstance().liveStreamManager.setAudioStreamingEnabled(false)
        DJISDKManager.getInstance().liveStreamManager.setVideoSource(LiveStreamManager.LiveStreamVideoSource.Primary)
        StartStreaming()

    }

    private fun initListener() {
        listener = OnLiveChangeListener {
                i ->
            run {
                if (i == 0) {
                    setResultToToast("Stream started successfully")
                } else {
                    setResultToToast("Stream initialisation failed")
                }
            }
        }
    }
}