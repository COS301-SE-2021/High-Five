package com.bdpsolutions.highfive.utils

object GetTimestamp {
    operator fun invoke(offset: Int?): Long {
        return (System.currentTimeMillis() / 1000L) + (offset ?: 0)
    }
}