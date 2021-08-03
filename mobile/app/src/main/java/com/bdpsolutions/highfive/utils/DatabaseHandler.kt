package com.bdpsolutions.highfive.utils

import android.content.Context
import androidx.room.Room

/**
 * This object manages an instance to the Room database. This ensures that only one instance is
 * available to the application.
 */
object DatabaseHandler {
    private lateinit var database: AppDatabase
    private var isMade = false

    fun getDatabase(ctx: Context): AppDatabase {
        if (!isMade) {
            database = Room.databaseBuilder(
                ctx,
                AppDatabase::class.java, "app-database"
            ).build()
            isMade = true
        }
        return database
    }
}