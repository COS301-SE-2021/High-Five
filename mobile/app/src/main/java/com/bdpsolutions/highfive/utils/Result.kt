package com.bdpsolutions.highfive.utils

/**
 * A generic class that holds a value with its loading status.
 * @param <T>
 */
sealed class Result<out T : Any> {

    data class Success<out T : Any>(val data: T) : Result<T>()
    data class Error(val exception: Exception) : Result<Nothing>()

    override fun toString(): String {
        return when (this) {
            is Success<*> -> "Success[data=$data]"
            is Error -> "Error[exception=$exception]"
        }
    }

    fun getResult(): T {
        if (this is Success<*>) {
            return data as T
        }
        throw java.lang.Exception("Cannot get data from failed result")
    }
}