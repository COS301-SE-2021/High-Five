package com.bdpsolutions.highfive.subsystems.image.model.source

import android.graphics.Bitmap
import android.util.Log
import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.constants.Errors
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.*
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import com.bdpsolutions.highfive.utils.*
import com.google.gson.GsonBuilder
import okhttp3.MediaType
import retrofit2.*
import retrofit2.converter.gson.GsonConverterFactory
import okhttp3.MultipartBody

import okhttp3.RequestBody
import java.io.File


class APIImageDataSource private constructor(): ImageDataSource {

    override fun fetchAllImages(imageObservable: MutableLiveData<ImageResult>) {
        val gson = GsonBuilder()
            .registerTypeHierarchyAdapter(
                ImageInfo::class.java,
                RetrofitDeserializers.ImageInfoDeserializer
            ).create()

        // create Retrofit object and fetch data
        val retrofit = Retrofit.Builder()
            .baseUrl(Endpoints.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()

        val imageSource = retrofit.create(ImageInfoEndpoint::class.java)
        val db = DatabaseHandler.getDatabase(null).userDao()
        ConcurrencyExecutor.execute {

            val bearer = db.getUser()?.authToken

            val call = imageSource.getAllImages("Bearer $bearer")

            call.enqueue(object : Callback<ImageList> {

                override fun onResponse(
                    call: Call<ImageList>,
                    response: Response<ImageList>
                ) {

                    if (response.isSuccessful) {
                        imageObservable.postValue(ImageResult( success = Result.Success(response.body()!!)))
                    } else {
                        Log.e("Error", response.message())
                        imageObservable.postValue(ImageResult( error = Errors.IMAGE.UNKNOWN_ERROR))
                    }
                }

                override fun onFailure(call: Call<ImageList>, t: Throwable) {
                    Log.e("TOKEN", "Failed to fetch image data: ${t.message}")
                }
            })
        }
    }

    override fun loadImage(image: File, callback: (() -> Unit)?) {
        val requestFile: RequestBody =
            RequestBody.create(MediaType.parse("multipart/form-data"), image)

        val body = MultipartBody.Part.createFormData("file", image.name, requestFile)

        val gson = GsonBuilder()
            .registerTypeHierarchyAdapter(
                ImageUploadResult::class.java,
                RetrofitDeserializers.ImageUploadResultDeserializer
            ).create()

        // create Retrofit object and fetch data
        val retrofit = Retrofit.Builder()
            .baseUrl(Endpoints.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()

        val imageSource = retrofit.create(ImageInfoEndpoint::class.java)

        ConcurrencyExecutor.execute {
            val db = DatabaseHandler.getDatabase(null).userDao()
            val bearer = db.getUser()?.authToken
            val call = imageSource.storeImage(
                authHeader = "Bearer $bearer",
                file = body
            )

            call.enqueue(object : Callback<ImageUploadResult> {
                override fun onResponse(
                    call: Call<ImageUploadResult>,
                    response: Response<ImageUploadResult>
                ) {

                    if (response.isSuccessful) {
                        Log.d("Image upload", "Success")
                        callback?.let { it() }
                    } else {
                        Log.e("Image upload", "Error: ${response.message()}")
                    }
                }

                override fun onFailure(call: Call<ImageUploadResult>, t: Throwable) {
                    Log.e("Image upload", "Failed to fetch image data: ${t.message}")
                }
            })
        }
    }

    companion object {
        fun create(): APIImageDataSource {
            return APIImageDataSource()
        }
    }
}