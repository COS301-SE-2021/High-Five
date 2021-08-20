package com.bdpsolutions.highfive.utils

import android.content.Context
import android.content.pm.PackageManager
import android.os.Bundle
import com.bdpsolutions.highfive.utils.factories.authConverterFactory

class AzureConfiguration {

    private var mClientId: String? = null
    val clientId: String
        get() = mClientId!!

    private var mScope: String? = null
    val scope: String
        get() = mScope!!

    private var mRedirectUri: String? = null
    val redirectUri: String
        get() = mRedirectUri!!

    private var mDiscoveryUri: String? = null
    val discoveryUri: String
        get() = mDiscoveryUri!!

    private var mCodeVerifier: String? = null
    var codeVerifier: String
        get() = mCodeVerifier!!
        set(value) {
            mCodeVerifier = value
        }


    companion object {

        private var mInstance: AzureConfiguration? = null

        fun getInstance(): AzureConfiguration {
            if (mInstance == null) {
                val bundle: Bundle = ContextHolder.appContext!!.packageManager
                    .getApplicationInfo(
                        ContextHolder.appContext!!.packageName,
                        PackageManager.GET_META_DATA
                    ).metaData

                mInstance = AzureConfiguration()
                mInstance?.mClientId = authConverterFactory<String>("client_id", bundle)
                mInstance?.mScope = authConverterFactory<String>("authorization_scope", bundle)
                mInstance?.mRedirectUri = authConverterFactory<String>("redirect_uri", bundle)
                mInstance?.mDiscoveryUri = authConverterFactory<String>("discovery_uri", bundle)
            }
            return mInstance!!
        }
    }
}