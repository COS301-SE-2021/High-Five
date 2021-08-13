package com.bdpsolutions.highfive.subsystems.login.model.source

import android.content.Intent
import android.net.Uri
import androidx.activity.result.ActivityResultLauncher
import com.bdpsolutions.highfive.constants.TodoStatements

class DatabaseLogin: LoginDataSource {
    override fun login(resultLauncher: ActivityResultLauncher<Intent>) {

        //For now, just launch the result launcher with test data.
        //Eventually, this must read from a Room database for an auth code (or auth token)
        val intent = Intent()
        intent.data = Uri.parse("com.bdpsolutions.highfive://oauth2redirect/?state=test&code=test")
        resultLauncher.launch(intent)
    }

    override fun logout() {
        TODO(TodoStatements.NOT_YET_IMPLEMENTED)
    }
}