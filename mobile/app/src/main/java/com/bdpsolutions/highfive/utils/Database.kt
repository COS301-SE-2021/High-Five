package com.bdpsolutions.highfive.utils

import androidx.room.Database
import androidx.room.RoomDatabase
import com.bdpsolutions.highfive.models.login.model.User
import com.bdpsolutions.highfive.models.login.model.UserDao

@Database(entities = [User::class], version = 1)
abstract class AppDatabase : RoomDatabase() {
    abstract fun userDao(): UserDao
}
