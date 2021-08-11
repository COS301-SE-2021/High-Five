package com.bdpsolutions.highfive.utils

import android.content.Context
import com.android.volley.Request
import com.android.volley.RequestQueue
import com.android.volley.toolbox.Volley

class VolleyNetworkManager constructor(context: Context?) {
    companion object {
        @Volatile
        private var INSTANCE: VolleyNetworkManager? = null
        fun getInstance(context: Context?) =
            INSTANCE ?: synchronized(this) {
                INSTANCE ?: VolleyNetworkManager(context).also {
                    INSTANCE = it
                }
            }
    }
    val requestQueue: RequestQueue by lazy {

        //Assert not null because this gets initialized in the main application
        Volley.newRequestQueue(context!!.applicationContext)
    }
    fun <T> addToRequestQueue(req: Request<T>) {
        requestQueue.add(req)
    }

}