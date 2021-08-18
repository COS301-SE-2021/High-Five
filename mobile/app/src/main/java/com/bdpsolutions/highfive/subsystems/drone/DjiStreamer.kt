package com.bdpsolutions.highfive.subsystems.drone

import android.util.Log
import dji.sdk.camera.VideoFeeder
import dji.sdk.sdkmanager.LiveStreamManager
import dji.sdk.sdkmanager.DJISDKManager




class DjiStreamer : Runnable {
    private val rtmpServerUrl = "rtmp//:192.168.11.174:1935"
    private var runner = Thread(this)
    private val l = DJISDKManager.getInstance().liveStreamManager
    fun startStream(){
        runner.start()
    }

    override fun run() {
        Log.d("MavicMini", "LiveStream:live_streaming_start:$rtmpServerUrl")
        l.registerListener { x -> Log.d("MavicMini", "LiveStream callback:$x") }
        l.setVideoSource(LiveStreamManager.LiveStreamVideoSource.Primary)
        l.setVideoEncodingEnabled(true)
        l.setLiveUrl(rtmpServerUrl)
        var r = 0
        r = l.startStream()
    }
}