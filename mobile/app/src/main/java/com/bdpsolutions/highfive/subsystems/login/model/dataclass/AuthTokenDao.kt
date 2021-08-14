package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*

/**
 * DAO interface used by Room to run queries on the Room database
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 * @see AuthToken
 */
@Dao
interface AuthTokenDao {
    @Query("SELECT * FROM authtoken LIMIT 1")
    fun getUser(): AuthToken?

    @Query("UPDATE authtoken SET auth_token=(:token) WHERE uid=(SELECT uid from authtoken LIMIT 1)")
    fun updateToken(token: String)

    @Insert
    fun addUser(authToken: AuthToken)
}
