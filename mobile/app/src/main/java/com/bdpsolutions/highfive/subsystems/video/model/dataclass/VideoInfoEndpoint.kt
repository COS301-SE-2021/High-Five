package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import androidx.room.Insert
import androidx.room.Query
import com.bdpsolutions.highfive.constants.Endpoints
import retrofit2.Call
import retrofit2.http.Header
import retrofit2.http.POST

/**
 * Data Access Object for VideoPreview data. This class has data access methods for
 * fetching and storing data via Room and Retrofit.
 */
interface VideoInfoEndpoint {
    @POST(Endpoints.VIDEO.GET_ALL_VIDEOS)
    fun getAllVideos(
        @Header("Authorization") authHeader: String
    ): Call<VideoList>

}