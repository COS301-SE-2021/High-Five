package com.bdpsolutions.highfive.subsystems.image.model.dataclass

import com.bdpsolutions.highfive.constants.Endpoints
import okhttp3.MultipartBody
import retrofit2.Call
import retrofit2.http.Header
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part

interface ImageInfoEndpoint {

    @POST(Endpoints.IMAGE.GET_ALL_IMAGES)
    fun getAllImages(
        @Header("Authorization") authHeader: String
    ): Call<ImageList>

    @Multipart
    @POST(Endpoints.IMAGE.STORE_IMAGE)
    fun storeImage(
        @Header("Authorization") authHeader: String,
        @Part file: MultipartBody.Part,
    ): Call<ImageUploadResult>
}