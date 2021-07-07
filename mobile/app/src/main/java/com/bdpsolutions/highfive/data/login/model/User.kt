package com.bdpsolutions.highfive.data.login.model

import androidx.room.*

@Entity
data class User(
    @PrimaryKey val uid: Int,
    @ColumnInfo(name = "auth_token") val token: String?
)