package com.high5.models.user;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.Query;

import java.util.List;

@Dao
public interface UserDao {

    @Query("SELECT * FROM user WHERE uid=(SELECT uid FROM user LIMIT 1) LIMIT 1")
    User getUser();

    @Query("UPDATE user set auth_token=(:token) where uid=(SELECT uid FROM user LIMIT 1)")
    void updateToken(String token);

    @Insert
    void insertAll(User... users);

    @Delete
    void delete(User user);
}
