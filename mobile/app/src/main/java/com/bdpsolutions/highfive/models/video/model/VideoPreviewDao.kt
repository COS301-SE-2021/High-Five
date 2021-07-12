package com.bdpsolutions.highfive.models.video.model

import androidx.room.Insert
import androidx.room.Query

interface VideoPreviewDao {
    @Query("SELECT * FROM videopreview")
    fun getVideoPreviews() : List<VideoPreview>

    @Insert
    fun addVideoPreview(vararg videoPreviews : VideoPreview)

    @Query("DELETE FROM videopreview WHERE id=(:id)")
    fun deleteVideoPreview(id: String)
}