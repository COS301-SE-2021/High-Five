package com.bdpsolutions.highfive.models.video.model

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import java.util.*

@Entity
data class VideoPreview(
    @PrimaryKey val id: String,
    @ColumnInfo(name = "name") val name: String,
    @ColumnInfo(name = "duration") val duration: Long,
    @ColumnInfo(name = "date_stored") val dateStored: Date,
    @ColumnInfo(name = "thumbnail") val thumbnail: String
)