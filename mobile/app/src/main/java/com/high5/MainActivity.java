package com.high5;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import com.high5.database.DatabaseController;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Creates an instance of the DatabaseController.
        DatabaseController.getInstance(getApplicationContext());
    }
}