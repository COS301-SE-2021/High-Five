package com.high5.database;

import androidx.room.Database;
import androidx.room.RoomDatabase;

import com.high5.models.user.User;
import com.high5.models.user.UserDao;

@Database(entities = {User.class}, version = 1)
public abstract class AppDatabase extends RoomDatabase {
    public abstract UserDao userDao();
}
