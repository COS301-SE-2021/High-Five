package com.bdpsolutions.highfive.subsystems.login.model.dataclass

import androidx.room.*
import com.bdpsolutions.highfive.BuildConfig
import retrofit2.http.GET

@Dao
interface UserDao {
    @Query("SELECT * FROM user LIMIT 1")
    fun getUser(): User?

    @Query("UPDATE user SET auth_token=(:token) WHERE uid=(SELECT uid from user LIMIT 1)")
    fun updateToken(token: String)

    @Insert
    fun addUser(user: User)
}
