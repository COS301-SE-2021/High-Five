package com.bdpsolutions.highfive.models.video

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.models.video.source.VideoDataSource
import com.bdpsolutions.highfive.utils.Result

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
 * @param apiVideoSource API endpoints to access data
 * @param dbVideoSource Database endpoints to access data
 */
class VideoDataRepository(private val apiVideoSource: VideoDataSource,
                          private val dbVideoSource: VideoDataSource) {

    /**
     * Fetches new video preview data from the backend service.
     *
     * @param callback A callback function that takes a list of video preview data as an argument.
     */
    fun refreshData(callback: (Result<List<VideoPreview>>) -> Unit) {
        apiVideoSource.getVideoPreviewData(callback)
    }
}