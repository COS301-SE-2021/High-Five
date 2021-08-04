package com.bdpsolutions.highfive.models.video.model

import androidx.room.Insert
import androidx.room.Query
import com.bdpsolutions.highfive.constants.Endpoints
import retrofit2.Call
import retrofit2.http.POST

/**
 * Data Access Object for VideoPreview data. This class has data access methods for
 * fetching and storing data via Room and Retrofit.
 */
interface VideoPreviewDao {

    //Room queries

    @Query("SELECT * FROM videopreview")
    fun getVideoPreviews() : List<VideoPreview>

    @Insert
    fun addVideoPreview(vararg videoPreviews : VideoPreview)

    @Query("DELETE FROM videopreview WHERE id=(:id)")
    fun deleteVideoPreview(id: String)

    //Retrofit Queries

    @POST(Endpoints.VIDEO.GET_ALL_VIDEOS)
    fun getVideoPreviewData() : Call<List<VideoPreview>>
}