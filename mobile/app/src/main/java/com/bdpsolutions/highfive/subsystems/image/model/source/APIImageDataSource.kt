package com.bdpsolutions.highfive.subsystems.image.model.source

import android.util.Log
import androidx.lifecycle.MutableLiveData
import com.bdpsolutions.highfive.constants.Endpoints
import com.bdpsolutions.highfive.constants.Errors
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.*
import com.bdpsolutions.highfive.subsystems.image.viewmodel.ImageResult
import com.bdpsolutions.highfive.utils.*
import com.google.gson.GsonBuilder
import retrofit2.*
import retrofit2.converter.gson.GsonConverterFactory

class APIImageDataSource : ImageDataSource {

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

            call.enqueue(object : Callback<List<ImageInfo>> {

                override fun onResponse(
                    call: Call<List<ImageInfo>>,
                    response: Response<List<ImageInfo>>
                ) {

                    if (response.isSuccessful) {
                        imageObservable.postValue(ImageResult( success = Result.Success(response.body()!!)))
                    } else {
                        Log.e("Error", response.message())
                        imageObservable.postValue(ImageResult( error = Errors.IMAGE.UNKNOWN_ERROR))
                    }
                }

                override fun onFailure(call: Call<List<ImageInfo>>, t: Throwable) {
                    Log.e("TOKEN", "Failed to fetch image data: ${t.message}")
                }
            })
        }
    }
}