package com.bdpsolutions.highfive.utils.factories

import android.os.Bundle
import android.text.TextUtils
import com.google.gson.JsonElement
import java.lang.Exception
import java.text.SimpleDateFormat
import java.util.*

/**
 * Factory function to convert a JSON element to a given type.
 *
 * Returns null if the JSON element is null or the given type is unsupported
 */
inline fun <reified T> authConverterFactory (name: String, bundle: Bundle) :T? {

    return when (T::class) {
        String::class -> {
            var value = bundle.getString(name) ?: throw Exception("No such value found")
            value = value.trim { it <= ' ' }
            value = value.replace("&&&", " ")
            if (TextUtils.isEmpty(value)) {
                "" as T
            } else value as T
        }
        Boolean::class -> bundle.getBoolean(name) as T
        else -> throw Exception("Unsupported class")
    }
}