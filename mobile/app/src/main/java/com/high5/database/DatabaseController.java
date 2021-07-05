package com.high5.database;

import android.content.Context;

import androidx.room.Room;

/**
 * This class is used to manage instances of the Room database.
 *
 * This class was made under the recommendation from this source:
 * https://developer.android.com/training/data-storage/room#database
 */
public class DatabaseController {
    private static DatabaseController instance;
    private final AppDatabase database;

    private DatabaseController(AppDatabase database) {
        this.database = database;
    }

    /**
     * Returns the Room database instance
     *
     * @return Room database
     */
    public AppDatabase getDatabase() {
        return database;
    }

    /**
     * Fetches an instance of the database controller. If one does not exist, it will create a new instance,
     * while creating the Room database for the rest of the application to use.
     *
     * @param context Needed by Room to build a new database instance. It is not used if an instance already exists.
     * @return an instance of the database controller.
     */
    public static DatabaseController getInstance(Context context){
        if (instance == null) {
            AppDatabase db = Room.databaseBuilder(context,
                    AppDatabase.class, "app-data").build();
            instance = new DatabaseController(db);
        }
        return instance;
    }
}
