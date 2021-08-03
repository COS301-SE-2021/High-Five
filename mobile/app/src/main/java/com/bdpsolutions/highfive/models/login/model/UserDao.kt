package com.bdpsolutions.highfive.models.login.model

import androidx.room.*

@Dao
interface UserDao {
    @Query("SELECT * FROM user LIMIT 1")
    fun getUser(): User?

    @Query("UPDATE user SET auth_token=(:token) WHERE uid=(SELECT uid from user LIMIT 1)")
    fun updateToken(token: String)

    @Insert
    fun addUser(user: User)
}
