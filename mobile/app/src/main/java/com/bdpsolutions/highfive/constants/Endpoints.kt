package com.bdpsolutions.highfive.constants

object Endpoints {

    const val BASE_URL = "https://high5api.azurewebsites.net"

    object VIDEO {
        const val GET_ALL_VIDEOS = "/media/getAllVideos"
    }

    object AUTH {
        const val AUTH_TOKEN = "/oauth2/v2.0/token"
    }
}