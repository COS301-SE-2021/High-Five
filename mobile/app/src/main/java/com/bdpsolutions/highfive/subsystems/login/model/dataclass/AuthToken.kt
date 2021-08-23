package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*

/**
 * Room data class to be made as a table. This class gets populated with access tokens from Azure.
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 */
@Entity
data class AuthToken(
    @PrimaryKey val uid: Int,
    @ColumnInfo(name = "auth_token") val authToken: String?,
    @ColumnInfo(name = "refresh_token") val refreshToken: String?,
    @ColumnInfo(name = "auth_expires") val authExpires: Long?,
    @ColumnInfo(name = "refresh_expires") val refreshExpires: Long?
)