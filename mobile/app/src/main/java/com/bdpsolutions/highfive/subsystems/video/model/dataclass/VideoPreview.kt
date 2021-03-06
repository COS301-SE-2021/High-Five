package com.bdpsolutions.highfive.subsystems.video.model.dataclass

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import com.google.gson.annotations.SerializedName
import java.util.*

@Entity
data class VideoPreview(
    @PrimaryKey val id : String?,
    @ColumnInfo(name = "name") val name: String?,
    @ColumnInfo(name = "duration") val duration: Long?,
    @ColumnInfo(name = "date_stored") val dateStored: Date?,
    @ColumnInfo(name = "thumbnail") val thumbnail: String?
)