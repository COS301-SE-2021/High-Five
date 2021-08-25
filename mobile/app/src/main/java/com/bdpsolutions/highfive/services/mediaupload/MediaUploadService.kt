package com.bdpsolutions.highfive.services.mediaupload

import android.app.Service
import android.content.Intent
import android.os.*
import android.os.Process.THREAD_PRIORITY_BACKGROUND
import com.bdpsolutions.highfive.services.mediaupload.uploader.MediaUploader
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject
import androidx.localbroadcastmanager.content.LocalBroadcastManager




@AndroidEntryPoint
class MediaUploadService : Service() {
    private var serviceLooper: Looper? = null
    private var serviceHandler: ServiceHandler? = null

    @Inject
    lateinit var repositoryFactory: RepositoryFactory

    private val uploader: MediaUploader by lazy {
        MediaUploader(repositoryFactory)
    }

    // Handler that receives messages from the thread
    private inner class ServiceHandler(looper: Looper) : Handler(looper) {

        override fun handleMessage(msg: Message) {
            // Normally we would do some work here, like download a file.
            // For our sample, we just sleep for 5 seconds.
            try {
                Thread.sleep(5000L)
                val type = msg.data.getString("media_type")
                val path = msg.data.getString("media_file")
                val returnMessage = msg.data.getString("return")

                uploader.setMediaType(type!!).upload(path!!) {
                    val intent = Intent(returnMessage)

                    intent.putExtra("success", "Media uploaded")
                    LocalBroadcastManager.getInstance(this@MediaUploadService).sendBroadcast(intent)
                }

            } catch (e: InterruptedException) {
                // Restore interrupt status.
                Thread.currentThread().interrupt()
            }

            // Stop the service using the startId, so that we don't stop
            // the service in the middle of handling another job
            stopSelf(msg.arg1)
        }
    }

    override fun onCreate() {
        // Start up the thread running the service.  Note that we create a
        // separate thread because the service normally runs in the process's
        // main thread, which we don't want to block.  We also make it
        // background priority so CPU-intensive work will not disrupt our UI.
        super.onCreate()
        HandlerThread("ServiceStartArguments", THREAD_PRIORITY_BACKGROUND).apply {
            start()

            // Get the HandlerThread's Looper and use it for our Handler
            serviceLooper = looper
            serviceHandler = ServiceHandler(looper)
        }
    }

    override fun onStartCommand(intent: Intent, flags: Int, startId: Int): Int {
        // For each start request, send a message to start a job and deliver the
        // start ID so we know which request we're stopping when we finish the job

        serviceHandler?.obtainMessage()?.also { msg ->
            msg.arg1 = startId
            msg.data = intent.extras
            serviceHandler?.sendMessage(msg)
        }

        // If we get killed, after returning from here, restart
        return START_STICKY
    }

    override fun onBind(intent: Intent): IBinder? {
        // We don't provide binding, so return null
        return null
    }

    //TODO: Send result back to main application
    override fun onDestroy() {
        super.onDestroy()
    }

}