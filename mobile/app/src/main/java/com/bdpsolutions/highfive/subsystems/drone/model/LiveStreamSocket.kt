package com.bdpsolutions.highfive.subsystems.drone.model

import android.util.Log
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.DatabaseHandler
import com.google.gson.Gson
import org.java_websocket.client.WebSocketClient
import org.java_websocket.handshake.ServerHandshake
import java.lang.Exception
import java.net.URI

class LiveStreamSocket(endpoint: URI, private val callback: (String) -> Unit) : WebSocketClient(endpoint) {

    private var state : Int = 0;

    override fun onOpen(handshakedata: ServerHandshake?) {
        Log.i("LiveStreamSocket", "Opened new WebSocket connection")
        ConcurrencyExecutor.execute {
            //NOTE: This assumes that the refresh token is in the database
            val db = DatabaseHandler.getDatabase(null).userDao()
            val refreshToken = db.getUser()?.authToken!!

            val request = SocketRequest(
                authorization = refreshToken,
                request = "Synchronize"
            )
            send(Gson().toJson(request))
        }
    }

    override fun onMessage(message: String?) {

        //The first message it'll receive will be an acknowledgement following a Synchronisation request,
        //so the response can be ignored and we send a liveAnalysis request
        if (state == 0) {
            ConcurrencyExecutor.execute {
                //NOTE: This assumes that the refresh token is in the database
                val db = DatabaseHandler.getDatabase(null).userDao()
                val refreshToken = db.getUser()?.authToken!!

                val request = SocketRequest(
                    authorization = refreshToken,
                    request = "StartLiveAnalysis"
                )
                send(Gson().toJson(request))
            }
            state++;
        } else {
            //Deserialize response and send url to callback.
        }
    }

    override fun onClose(code: Int, reason: String?, remote: Boolean) {
        Log.i("LiveStreamSocket", "Exited socket with code: $code, Reason: $reason")
    }

    override fun onError(ex: Exception?) {
        Log.e("LiveStreamSocket", "${ex?.message}")
    }
}