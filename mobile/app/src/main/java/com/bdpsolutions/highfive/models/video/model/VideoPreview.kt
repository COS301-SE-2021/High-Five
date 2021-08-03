package com.bdpsolutions.highfive.models.video.model

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import com.google.gson.annotations.SerializedName
import java.util.*

@Entity
data class VideoPreview(
    @PrimaryKey
    @SerializedName("id")
    val id: String,

    @ColumnInfo(name = "name")
    @SerializedName("name")
    val name: String,
    @ColumnInfo(name = "duration")
    @SerializedName("duration")
    val duration: Long,
    @ColumnInfo(name = "date_stored")
    @SerializedName("dateStored")
    val dateStored: Date,
    @ColumnInfo(name = "thumbnail")
    @SerializedName("thumbnail")
    val thumbnail: String
)