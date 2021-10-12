package com.bdpsolutions.highfive.subsystems.drone.model

import android.util.Log
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.DatabaseHandler
import com.bdpsolutions.highfive.utils.ToastUtils
import com.google.gson.Gson
import com.google.gson.JsonObject
import io.reactivex.rxjava3.core.Observer
import org.java_websocket.client.WebSocketClient
import org.java_websocket.handshake.ServerHandshake
import java.lang.Exception
import java.net.URI
import java.util.concurrent.locks.ReentrantLock

class LiveStreamSocket(private val requestType: String,
                       endpoint: URI,
                       private val toastObserver: Observer<String>,
                       private val callback: (String) -> Unit) : WebSocketClient(endpoint)
{

    private var state : Int = 0
    private val lock: ReentrantLock = ReentrantLock()

    override fun onOpen(handshakedata: ServerHandshake?) {
        Log.i("LiveStreamSocket", "Opened new WebSocket connection")
        ConcurrencyExecutor.execute {
            //NOTE: This assumes that the refresh token is in the database
            val db = DatabaseHandler.getDatabase(null).userDao()
            val refreshToken = db.getUser()?.authToken!!

            val request = SocketRequest(
                authorization = refreshToken,
                request = "Synchronize",
                body = "{}"
            )
            Log.d("LiveStreamSocket", Gson().toJson(request))
            //ToastUtils.showToast(Gson().toJson(request))
            send(Gson().toJson(request))
        }
    }

    override fun onMessage(message: String?) {

        lock.lock()

        try {

            Log.d("LiveStreamSocket", "$message")

            //The first message it'll receive will be an acknowledgement following a Synchronisation request,
            //so the response can be ignored and we send a liveAnalysis request
            if (state == 0) {
                ConcurrencyExecutor.execute {
                    //NOTE: This assumes that the refresh token is in the database
                    val db = DatabaseHandler.getDatabase(null).userDao()
                    val refreshToken = db.getUser()?.authToken!!

                    val request = SocketRequest(
                        authorization = refreshToken,
                        request = requestType,
                        body = "{}"
                    )
                    send(Gson().toJson(request))
                    //callback("rtmp://highfiveanalysis.ddns.net/55799ed725ac42bcbb1925c715380541/070482602661397257359202")
                }
                state++
            } else {
                val initResponse = Gson().fromJson(message, JsonObject::class.java)
                val response = Gson().fromJson(initResponse.get("message"), SocketResponse::class.java)
                Log.d("LiveStreamSocket", "got response")
                //ToastUtils.showToast("Got response")
                if (response.status == "success") {
                    if (response.publishLink != null && response.publishLink != "none") {
                        callback(response.publishLink!!)
                    }
                } else {
                    Log.d("LiveStreamSocket", "${response.message}")
                    toastObserver.onNext("Error: ${response.message}")
                    ToastUtils.showToast(response.message)
                }
            }
        } finally {
            lock.unlock()
        }
    }

    override fun onClose(code: Int, reason: String?, remote: Boolean) {
        Log.i("LiveStreamSocket", "Exited socket with code: $code, Reason: $reason")
    }

    override fun onError(ex: Exception?) {
        Log.e("LiveStreamSocket", "${ex?.message}")
    }
}