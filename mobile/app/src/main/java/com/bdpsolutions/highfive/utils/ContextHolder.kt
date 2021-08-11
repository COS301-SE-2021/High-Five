package com.bdpsolutions.highfive.utils

import android.app.Activity

object ContextHolder {
    var appContext: Activity? = null
        set(value) {
            if (field == null) {
                field = value
            }
            field = value
        }

        get() {
            return field!!
        }
}