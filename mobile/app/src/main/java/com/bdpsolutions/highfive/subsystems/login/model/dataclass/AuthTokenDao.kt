package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*

@Dao
interface AuthTokenDao {
    @Query("SELECT * FROM authtoken LIMIT 1")
    fun getUser(): AuthToken?

    @Query("UPDATE authtoken SET auth_token=(:token) WHERE uid=(SELECT uid from authtoken LIMIT 1)")
    fun updateToken(token: String)

    @Insert
    fun addUser(authToken: AuthToken)
}
