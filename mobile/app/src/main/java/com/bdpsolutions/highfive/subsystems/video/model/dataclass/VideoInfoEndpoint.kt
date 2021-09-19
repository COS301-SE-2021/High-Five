package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import com.bdpsolutions.highfive.constants.Endpoints
import okhttp3.MultipartBody
import retrofit2.Call
import retrofit2.http.*

/**
 * Data Access Object for VideoPreview data. This class has data access methods for
 * fetching and storing data via Room and Retrofit.
 */
interface VideoInfoEndpoint {
    @GET(Endpoints.VIDEO.GET_ALL_VIDEOS)
    fun getAllVideos(
        @Header("Authorization") authHeader: String
    ): Call<VideoList>

    @Multipart
    @POST(Endpoints.VIDEO.STORE_VIDEO)
    fun storeVideo(
        @Header("Authorization") authHeader: String,
        @Part file: MultipartBody.Part,
    ): Call<VideoUploadResult>
}