package com.bdpsolutions.highfive.utils.retrofit

import okhttp3.OkHttpClient
import java.util.concurrent.TimeUnit

object OkHttpConfiguration {
    val configuration: OkHttpClient = OkHttpClient().newBuilder()
        .connectTimeout(120, TimeUnit.SECONDS)
        .readTimeout(120, TimeUnit.SECONDS)
        .writeTimeout(120, TimeUnit.SECONDS)
        .build()
}