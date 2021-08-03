package com.bdpsolutions.highfive.models.video.source

import android.util.Log
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.models.video.model.VideoPreviewDao
import com.bdpsolutions.highfive.utils.*
import com.google.gson.*
import retrofit2.*
import retrofit2.converter.gson.GsonConverterFactory

/**
 * API implementation for the VideoDataSource. This class fetches data from the backend service.
 */
class APIVideoDataSource : VideoDataSource {

    /**
     * Fetches video preview data from the backend service. This data is passed to a callback
     * function
     *
     * @param callback Callback function that will run once the data are fetched
     */
    override fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit) {

        // create a deserializer to create the VideoPreview objects
        val gson = GsonBuilder()
            .registerTypeHierarchyAdapter(
                VideoPreview::class.java,
                RetrofitDeserializers.VideoPreviewDeserializer
            ).create()

        // create Retrofit object and fetch data
        val retrofit = Retrofit.Builder()
            .baseUrl(Endpoints.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()

        val videoSource = retrofit.create(VideoPreviewDao::class.java)
        val call = videoSource.getVideoPreviewData()

        // Enqueue callback object that will call the callback function passed to this function
        call.enqueue(object : Callback<List<VideoPreview>> {

            override fun onResponse(
                call: Call<List<VideoPreview>>,
                response: Response<List<VideoPreview>>
            ) {

                if (response.isSuccessful) {
                    callback(Result.Success(response.body()!!))
                } else {
                    Log.e("Error", response.message())
                }
            }

            override fun onFailure(call: Call<List<VideoPreview>>, t: Throwable) {
                throw t
            }
        })
    }
}