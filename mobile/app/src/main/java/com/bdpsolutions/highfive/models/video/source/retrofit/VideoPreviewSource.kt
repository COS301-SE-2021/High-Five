package com.bdpsolutions.highfive.models.video.source.retrofit

import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.models.video.model.VideoPreview
import retrofit2.Call
import retrofit2.http.POST

interface VideoPreviewSource {
    @POST(Endpoints.VIDEO.GET_ALL_VIDEOS)
    fun getVideoPreviewData() : Call<List<VideoPreview>>
}