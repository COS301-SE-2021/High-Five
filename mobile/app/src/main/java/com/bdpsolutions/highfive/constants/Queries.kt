package com.bdpsolutions.highfive.constants

object Queries {
    object AUTHENTICATION {
        //language=RoomSql
        const val UPDATE_TOKEN = "UPDATE authtoken SET auth_token=(:auth_token), refresh_token=(:refresh_token), auth_expires=(:auth_expires), refresh_expires=(:refresh_expires) WHERE uid=(SELECT uid from authtoken LIMIT 1)"

        //language=RoomSql
        const val GET_USER = "SELECT * FROM authtoken LIMIT 1"
    }
}