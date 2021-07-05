package com.high5;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;

import com.high5.activities.login.LoginActivity;
import com.high5.activities.main.AppActivity;
import com.high5.database.AppDatabase;
import com.high5.database.DatabaseController;
import com.high5.models.user.User;
import com.high5.utils.ThreadExecutor;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Creates an instance of the DatabaseController.
        AppDatabase controller = DatabaseController.getInstance(getApplicationContext()).getDatabase();


        //Check if the user has logged in previously in the app. As database access cannot happen in
        //the main thread, the check is done asynchronously.
        ThreadExecutor.execute(() -> {
            User user = controller.userDao().getUser();

            Intent intent;
            if (user == null) {
                intent = new Intent(this, LoginActivity.class);
            } else {
                intent = new Intent(this, AppActivity.class);
            }
            startActivity(intent);
            finish();
        });
    }
}