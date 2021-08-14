package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*

@Entity
data class AuthToken(
    @PrimaryKey val uid: Int,
    @ColumnInfo(name = "auth_token") val authToken: String?,
    @ColumnInfo(name = "refresh_token") val refreshToken: String?,
    @ColumnInfo(name = "auth_expires") val authExpires: Int?,
    @ColumnInfo(name = "refresh_expires") val refreshExpires: Int?
)