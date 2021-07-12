package com.bdpsolutions.highfive.models.video.source

import com.android.volley.Request
import com.android.volley.toolbox.*
import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.bdpsolutions.highfive.utils.*
import java.lang.Exception
import java.text.SimpleDateFormat
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import java.util.*
import kotlin.collections.ArrayList

class APIVideoDataSource : VideoDataSource {
    override fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit) {

        val request = JsonArrayRequest(Request.Method.POST, "https://high5api.azurewebsites.net/media/getAllVideos", null,
            { response ->
                val videoData = ArrayList<VideoPreview>()
                val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())

                for (i in 0..response.length()-1) {
                    videoData.add(
                        VideoPreview(
                            id = response.getJSONObject(i).getString("id"),
                            name = response.getJSONObject(i).getString("name"),
                            duration = response.getJSONObject(i).getLong("duration"),
                            dateStored = format.parse(response.getJSONObject(i).getString("dateStored"))!!,
                            thumbnail = response.getJSONObject(i).getString("thumbnail")
                        )
                    )
                }

                callback(Result.Success(videoData))
            },
            { error -> Result.Error(Exception(error.message)) })

        VolleyNetworkManager.getInstance(null).addToRequestQueue(request)
    }

    override fun addPreviewData(vararg data: VideoPreview) {
        TODO("Not yet implemented")
    }

    override fun deletePreviewData(id: String) {
        TODO("Not yet implemented")
    }
}