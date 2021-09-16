


package com.bdpsolutions.highfive.subsystems.drone

import android.util.Log
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.subsystems.drone.model.LiveStreamSocket
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.ToastUtils.setResultToToast
import dji.sdk.camera.VideoFeeder
import dji.sdk.camera.VideoFeeder.VideoDataListener
import dji.sdk.camera.VideoFeeder.VideoFeed
import dji.sdk.sdkmanager.LiveStreamManager
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.LiveStreamManager.OnLiveChangeListener
import java.net.URI


class DjiStreamer{
    private val l = DJISDKManager.getInstance().liveStreamManager
    fun DjiStreamer(){}
    var listener = LiveStreamManager.OnLiveChangeListener {  }

    private fun StartStreaming(url: String) {

        if (DJISDKManager.getInstance().liveStreamManager.isStreaming) {
            setResultToToast("already started the Stream!")
            return
        }

        ConcurrencyExecutor.execute {
            DJISDKManager.getInstance().liveStreamManager.liveUrl = url;
            val result = DJISDKManager.getInstance().liveStreamManager.startStream()
            DJISDKManager.getInstance().liveStreamManager.setStartTime()
        }
    }

    fun setupLiveStream() {
        initListener()
        DJISDKManager.getInstance().liveStreamManager.registerListener(listener)
        DJISDKManager.getInstance().liveStreamManager.setAudioStreamingEnabled(false)
        DJISDKManager.getInstance().liveStreamManager.setVideoSource(LiveStreamManager.LiveStreamVideoSource.Primary)
        ConcurrencyExecutor.execute {

            val webSocket = LiveStreamSocket(URI(Endpoints.WEBSOCKET_URL)) { response->
                StartStreaming(response)
            }

            webSocket.connect();
            while (! webSocket.isClosed) {
                Thread.sleep(1000L)
            }
        }
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