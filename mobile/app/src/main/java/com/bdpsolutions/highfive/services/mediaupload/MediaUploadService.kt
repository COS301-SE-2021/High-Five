package com.bdpsolutions.highfive.services.mediaupload

import android.app.*
import android.content.*
import android.os.*
import android.os.Process.THREAD_PRIORITY_BACKGROUND
import android.util.Log
import android.widget.Toast
import androidx.core.app.*
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.bdpsolutions.highfive.R
import com.bdpsolutions.highfive.services.mediaupload.uploader.MediaUploader
import com.bdpsolutions.highfive.utils.factories.RepositoryFactory
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject
import com.bdpsolutions.highfive.services.mediaupload.observers.MediaObserver
import com.bdpsolutions.highfive.utils.Result
import java.io.File


@AndroidEntryPoint
class MediaUploadService : Service() {
    private var serviceLooper: Looper? = null
    private var serviceHandler: ServiceHandler? = null

    private val CHANNEL_ID = "channel1"

    @Inject
    lateinit var repositoryFactory: RepositoryFactory

    private val uploader: MediaUploader by lazy {
        MediaUploader(repositoryFactory)
    }

    private val builder = NotificationCompat.Builder(this@MediaUploadService, CHANNEL_ID).apply {
        setContentTitle("Media Upload")
        setContentText("Upload in progress")
        setSmallIcon(R.mipmap.logo1_2)
        priority = NotificationCompat.PRIORITY_DEFAULT
    }

    private val notificationManager: NotificationManagerCompat by lazy {
        NotificationManagerCompat.from(this@MediaUploadService)
    }

    // Handler that receives messages from the thread
    private inner class ServiceHandler(looper: Looper) : Handler(looper) {

        override fun handleMessage(msg: Message) {
            // Normally we would do some work here, like download a file.
            // For our sample, we just sleep for 5 seconds.
            try {

                val type = msg.data.getString("media_type")
                val path = msg.data.getString("media_file")
                val returnMessage = msg.data.getString("return")

                createNotificationChannel()

                notificationManager.apply {
                    builder.setProgress(100, 0, false)
                    notify(1, builder.build())
                }

                val progressObserver = MediaObserver<Int>()

                progressObserver.setProgressCallback { progress ->

                    //Update the notification to reflect the current progress
                    //If the progress is at 100%, set to indeterminate mode.
                    run {
                        builder.setProgress(100, progress, progress == 100)
                        notificationManager.notify(1, builder.build())
                    }
                }

                progressObserver.setErrorCallback {
                    setFailedNotification()
                    Toast.makeText(this@MediaUploadService, "Failed to upload ${type}: ${it.message}", Toast.LENGTH_LONG).show()
                }

                val resultObserver = MediaObserver<Result<String>>()

                resultObserver.setProgressCallback {
                    when(it) {
                        is Result.Success<String> -> {

                            //Set the notification to 'Complete'
                            builder.setContentText("Upload complete")
                                .setProgress(0, 0, false)
                            notificationManager.notify(1, builder.build())

                            //Inform the main app that the file was uploaded
                            val intent = Intent(returnMessage)
                            intent.putExtra("success", "Media uploaded")
                            LocalBroadcastManager.getInstance(this@MediaUploadService).sendBroadcast(intent)

                            //delete cached file
                            val tmpFile = File(path!!)
                            if (tmpFile.exists()) {
                                tmpFile.delete()
                            }
                        }
                        is Result.Error -> {
                            setFailedNotification()
                            Toast.makeText(this@MediaUploadService, "Failed to upload ${type}: ${it.exception.message}", Toast.LENGTH_LONG).show()
                        }
                    }
                }

                resultObserver.setErrorCallback {
                    setFailedNotification()
                    Toast.makeText(this@MediaUploadService, "Failed to upload ${type}: ${it.message}", Toast.LENGTH_LONG).show()
                }

                uploader.setMediaType(type!!).upload(path!!, progressObserver, resultObserver)

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

    private fun createNotificationChannel() {
        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val name = "channelName"
            val descriptionText = "description"
            val importance = NotificationManager.IMPORTANCE_DEFAULT
            val channel = NotificationChannel(CHANNEL_ID, name, importance).apply {
                description = descriptionText
            }
            // Register the channel with the system
            val notificationManager: NotificationManager =
                getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            notificationManager.createNotificationChannel(channel)
        }
    }

    private fun setFailedNotification() {
        builder.setContentText("Upload failed")
            .setProgress(0, 0, false)
        notificationManager.notify(1, builder.build())
    }
}