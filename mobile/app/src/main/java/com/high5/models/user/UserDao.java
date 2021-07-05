package com.high5.models.user;

import androidx.room.*;

@Dao
public interface UserDao {

    @Query("SELECT * FROM user WHERE uid=(SELECT uid FROM user LIMIT 1) LIMIT 1")
    User getUser();

    @Query("UPDATE user SET auth_token=(:token) WHERE uid=(SELECT uid FROM user LIMIT 1)")
    void updateToken(String token);

    @Insert
    void createUser(User users);
}
