package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*

@Entity
data class User(
    @PrimaryKey val uid: Int,
    @ColumnInfo(name = "auth_token") val authToken: String?,
    @ColumnInfo(name = "refresh_token") val refreshToken: String?
)