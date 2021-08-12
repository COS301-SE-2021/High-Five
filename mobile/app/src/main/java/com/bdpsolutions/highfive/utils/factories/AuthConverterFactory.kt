package com.bdpsolutions.highfive.utils.factories

import android.os.Bundle
import android.text.TextUtils
import java.lang.Exception

import com.bdpsolutions.highfive.constants.Exceptions.AUTH_CONVERTER_FACTORY as auth

/**
 * Factory function to read a value from a Bundle.
 */
inline fun <reified T> authConverterFactory (name: String, bundle: Bundle) :T? {

    return when (T::class) {
        String::class -> {
            var value = bundle.getString(name) ?: throw Exception(auth.NO_SUCH_VALUE)
            value = value.trim { it <= ' ' }
            value = value.replace("&&&", " ")
            if (TextUtils.isEmpty(value)) {
                "" as T
            } else value as T
        }
        Boolean::class -> bundle.getBoolean(name) as T ?: throw Exception(auth.NO_SUCH_VALUE)
        else -> throw Exception(auth.UNSUPPORTED_CLASS)
    }
}