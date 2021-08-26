package com.bdpsolutions.highfive.subsystems.video.model.source

import android.util.Log
import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.constants.Errors
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageInfoEndpoint
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoInfo
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoInfoEndpoint
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoList
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoUploadResult
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoResult
import com.bdpsolutions.highfive.utils.ConcurrencyExecutor
import com.bdpsolutions.highfive.utils.DatabaseHandler
import com.bdpsolutions.highfive.utils.Result
import com.bdpsolutions.highfive.utils.RetrofitDeserializers
import com.bdpsolutions.highfive.utils.retrofit.CountingRequestBody
import com.google.gson.GsonBuilder
import io.reactivex.BackpressureStrategy
import io.reactivex.Flowable
import io.reactivex.Observer
import okhttp3.MediaType
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.File
import java.lang.Exception

/**
 * API implementation for the VideoDataSource. This class fetches data from the backend service.
 */
class APIVideoDataSource private constructor(): VideoDataSource {

    override fun fetchAllVideos(videoObservable: MutableLiveData<VideoResult>) {
        val gson = GsonBuilder()
            .registerTypeHierarchyAdapter(
                VideoInfo::class.java,
                RetrofitDeserializers.VideoInfoDeserializer
            ).create()

        // create Retrofit object and fetch data
        val retrofit = Retrofit.Builder()
            .baseUrl(Endpoints.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()

        val videoSource = retrofit.create(VideoInfoEndpoint::class.java)
        val db = DatabaseHandler.getDatabase(null).userDao()
        ConcurrencyExecutor.execute {

            val bearer = db.getUser()?.authToken

            val call = videoSource.getAllVideos("Bearer $bearer")

            call.enqueue(object : Callback<VideoList> {

                override fun onResponse(
                    call: Call<VideoList>,
                    response: Response<VideoList>
                ) {

                    if (response.isSuccessful) {
                        videoObservable.postValue(VideoResult( success = Result.Success(response.body()!!)))
                    } else {
                        Log.e("Error", response.message())
                        videoObservable.postValue(VideoResult( error = Errors.IMAGE.UNKNOWN_ERROR))
                    }
                }

                override fun onFailure(call: Call<VideoList>, t: Throwable) {
                    Log.e("TOKEN", "Failed to fetch video data: ${t.message}")
                }
            })
        }
    }

    override fun loadVideo(image: File,
                           progressObserver: Observer<Int>,
                           resultObserver: Observer<Result<String>>
    ) {
        val emitter: Flowable<Double> = Flowable.create({
            try {

                val requestFile: RequestBody = CountingRequestBody(
                    RequestBody.create(MediaType.parse("video/*"), image),
                    object : CountingRequestBody.Listener {
                        override fun onRequestProgress(bytesWritten: Long, contentLength: Long) {
                            val progress = (100.0 * bytesWritten / contentLength).toInt()
                            progressObserver.onNext(progress)
                        }
                    })

                val body = MultipartBody.Part.createFormData("file", image.name, requestFile)

                val gson = GsonBuilder()
                    .registerTypeHierarchyAdapter(
                        VideoUploadResult::class.java,
                        RetrofitDeserializers.VideoUploadResultDeserializer
                    ).create()

                // create Retrofit object and fetch data
                val retrofit = Retrofit.Builder()
                    .baseUrl(Endpoints.BASE_URL)
                    .addConverterFactory(GsonConverterFactory.create(gson))
                    .build()

                val videoSource = retrofit.create(VideoInfoEndpoint::class.java)

                ConcurrencyExecutor.execute {
                    val db = DatabaseHandler.getDatabase(null).userDao()
                    val bearer = db.getUser()?.authToken
                    val call = videoSource.storeVideo(
                        authHeader = "Bearer $bearer",
                        file = body
                    )

                    call.enqueue(object : Callback<VideoUploadResult> {
                        override fun onResponse(
                            call: Call<VideoUploadResult>,
                            response: Response<VideoUploadResult>
                        ) {

                            val result: Result<String> = if (response.isSuccessful) {
                                Result.Success("Success")
                            } else {
                                Result.Error(Exception(response.message()))
                            }
                            progressObserver.onComplete()
                            resultObserver.onNext(result)
                        }

                        override fun onFailure(call: Call<VideoUploadResult>, t: Throwable) {
                            resultObserver.onError(t)
                        }
                    })
                }
            } catch (e: Exception) {
                resultObserver.onError(e)
            }
        }, BackpressureStrategy.BUFFER)
        emitter.subscribe()
    }

    companion object {
        fun create(): APIVideoDataSource {
            return APIVideoDataSource()
        }
    }
}