package com.high5.models.user;

import com.high5.database.DatabaseController;

import java.io.IOException;

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
public class LoginDataSource {

    public Result<User> login(String username, String password) {

        try {
            User user = DatabaseController.getInstance(null).getDatabase().userDao().getUser();

            //TODO: authenticate with backend and get token from there.
            //TODO: create service that will do the authentication
            if (user == null) {
                user = new User("HELLO");
                DatabaseController.getInstance(null).getDatabase().userDao().createUser(user);
            }
            return new Result.Success<>(user);
        } catch (Exception e) {
            return new Result.Error(new IOException("Error logging in", e));
        }
    }

    public void logout() {
        // TODO: revoke authentication
    }
}