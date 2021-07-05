package com.high5.models.user;


import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;


/**
 * The User entity stores an authentication token to be used in API requests.
 */
@Entity
public class User {
    @PrimaryKey
    public int uid;

    @ColumnInfo(name = "auth_token")
    public String authToken;

}
