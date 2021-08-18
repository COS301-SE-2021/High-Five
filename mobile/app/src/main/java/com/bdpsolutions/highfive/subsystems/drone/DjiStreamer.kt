package com.bdpsolutions.highfive.subsystems.drone

import android.util.Log
import com.bdpsolutions.highfive.utils.ToastUtils.setResultToToast
import dji.sdk.camera.VideoFeeder
import dji.sdk.camera.VideoFeeder.VideoDataListener
import dji.sdk.camera.VideoFeeder.VideoFeed
import dji.sdk.sdkmanager.LiveStreamManager
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.LiveStreamManager.OnLiveChangeListener


class DjiStreamer{
    private val rtmpServerUrl = "rtmp//:192.168.11.174:1935"
    private val l = DJISDKManager.getInstance().liveStreamManager

    }
var listener = LiveStreamManager.OnLiveChangeListener {  }
    private fun setupLiveStream() {
        DJISDKManager.getInstance().liveStreamManager.registerListener(listener)
        initListener()
        DJISDKManager.getInstance().liveStreamManager.setAudioStreamingEnabled(false)
        DJISDKManager.getInstance().liveStreamManager.setVideoSource(LiveStreamManager.LiveStreamVideoSource.Primary)

    }

    private fun initListener() {
        listener = OnLiveChangeListener { i -> setResultToToast("status changed : $i") }
    }

    private fun StartStreaming() {

        if (DJISDKManager.getInstance().liveStreamManager.isStreaming) {
            setResultToToast("already started the Stream!")
            return
        }
        object : Thread() {
            override fun run() {
                DJISDKManager.getInstance().liveStreamManager.liveUrl = "rtmp//:192.168.11.174:1935"// + vehicleID);
                val result = DJISDKManager.getInstance().liveStreamManager.startStream()
                DJISDKManager.getInstance().liveStreamManager.setStartTime()
                setResultToToast(
                    """LiveStream Start: $result
 isVideoStreamSpeedConfigurable:${DJISDKManager.getInstance().liveStreamManager.isVideoStreamSpeedConfigurable}
 isLiveAudioEnabled:${DJISDKManager.getInstance().liveStreamManager.isLiveAudioEnabled}"""
                )
            }
        }.start()
    }

}