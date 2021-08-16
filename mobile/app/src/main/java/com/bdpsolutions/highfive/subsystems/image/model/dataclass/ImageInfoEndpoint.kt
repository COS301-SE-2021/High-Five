package com.bdpsolutions.highfive.subsystems.image.model.dataclass

import com.bdpsolutions.highfive.constants.Endpoints
import retrofit2.Call
import retrofit2.http.Header
import retrofit2.http.POST

interface ImageInfoEndpoint {

    @POST(Endpoints.IMAGE.GET_ALL_IMAGES)
    fun getAllImages(
        @Header("Authorization") authHeader: String
    ): Call<List<ImageInfo>>
}