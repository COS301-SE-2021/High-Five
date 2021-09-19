package com.bdpsolutions.highfive.constants

object Endpoints {

    const val BASE_URL = "https://high5api.azurewebsites.net/"
    const val WEBSOCKET_URL = "wss://high5api.azurewebsites.net/ws"

    object VIDEO {
        const val GET_ALL_VIDEOS = "media/getAllVideos"
        const val STORE_VIDEO = "media/storeVideo"
    }

    object IMAGE {
        const val GET_ALL_IMAGES = "media/getAllImages"
        const val STORE_IMAGE = "media/storeImage"
    }

    object AUTH {
        const val BASE_URL = "https://highfiveactivedirectory.b2clogin.com/highfiveactivedirectory.onmicrosoft.com/"
        const val AUTH_TOKEN = "B2C_1_signupsignin1/oauth2/v2.0/token"
    }
}