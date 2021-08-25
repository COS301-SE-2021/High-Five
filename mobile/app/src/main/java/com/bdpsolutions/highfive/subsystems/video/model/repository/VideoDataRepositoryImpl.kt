package com.bdpsolutions.highfive.subsystems.video.model.repository

import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.subsystems.video.model.source.VideoDataSource
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult
import java.io.File

/**
 * @author Kyle Barry (u19232510@tuks.co.za)
 *
 * Repository class to access Video data.
 *
 * Utilises two types of sources: an API source and a Database source.
 * The API source is for fetching new data from the backend, while the Database
 * source is used for caching data so that API calls do not have to be made
 * every time the view is loaded.
 *
 * As API calls need to happen asynchronously, the data sources will fetch data asynchronously.
 * As such, this repository takes in callback functions, which will run once data are fetched.
 *
 * To remain consistent, database access will use the same behaviour.
 *
 * @param source API endpoints to access data
 */
class VideoDataRepositoryImpl private constructor(source: VideoDataSource) : VideoDataRepository(source) {

    /**
     * Fetches new video preview data from the backend service.
     *
     * @param callback A callback function that takes a list of video preview data as an argument.
     */
    override fun fetchVideoData(videoObservable: MutableLiveData<VideoResult>) {
        source.fetchAllVideos(videoObservable)
    }

    override fun storeVideo(image: File, callback: (() -> Unit)?) {
        source.loadVideo(image, callback)
    }

    companion object {
        fun create(apiVideoSource: VideoDataSource): VideoDataRepositoryImpl {
            return VideoDataRepositoryImpl(apiVideoSource)
        }
    }
}