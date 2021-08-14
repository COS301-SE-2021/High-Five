package com.bdpsolutions.highfive.utils

import androidx.room.Database
import androidx.room.RoomDatabase
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AuthToken
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AuthTokenDao

@Database(entities = [AuthToken::class], version = 1)
abstract class AppDatabase : RoomDatabase() {
    abstract fun userDao(): AuthTokenDao
}
