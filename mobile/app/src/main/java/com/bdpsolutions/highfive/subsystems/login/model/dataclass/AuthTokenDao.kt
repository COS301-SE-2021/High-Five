package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*
import com.bdpsolutions.highfive.constants.Queries.AUTHENTICATION as auth

/**
 * DAO interface used by Room to run queries on the Room database
 *
 * @author Kyle Barry (u19232510@tuks.co.za)
 * @see AuthToken
 */
@Dao
interface AuthTokenDao {
    @Query(auth.GET_USER)
    fun getUser(): AuthToken?

    @Query(auth.UPDATE_TOKEN)
    fun updateToken(
        auth_token: String,
        refresh_token: String,
        auth_expires: Long,
        refresh_expires: Long
    )

    @Insert
    fun addUser(authToken: AuthToken)
}
