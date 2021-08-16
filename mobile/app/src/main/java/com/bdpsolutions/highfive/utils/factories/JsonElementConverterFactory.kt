package com.bdpsolutions.highfive.utils.factories

import android.net.Uri
import com.google.gson.JsonArray
import com.google.gson.JsonElement
import java.text.SimpleDateFormat
import java.util.*

/**
 * Factory function to convert a JSON element to a given type.
 *
 * Returns null if the JSON element is null or the given type is unsupported
 */
inline fun <reified T> jsonElementConverterFactory (element: JsonElement?) :T? {
    return if (element == null) {
        null
    } else {
        when (T::class) {
            String::class -> element.asString as T
            Long::class -> element.asLong as T
            Date::class -> {
                val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
                format.parse(element.asString) as T
            }
            Int::class -> element.asInt as T
            Uri::class -> Uri.parse(element.asString) as T
            JsonArray::class -> element.asJsonArray as T
            else -> null
        }
    }
}