package com.bdpsolutions.highfive.subsystems.video.model.source

import com.bdpsolutions.highfive.constants.TodoStatements
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.Result

/**
 * Database implementation for the VideoDataSource. Fetches data from a Room database instance.
 */
class DatabaseVideoDataSource private constructor(): VideoDataSource {
    override fun getVideoPreviewData(callback: (Result<List<VideoPreview>>) -> Unit) {
        TODO(TodoStatements.NOT_YET_IMPLEMENTED)
    }

    override fun addPreviewData(vararg data: VideoPreview) {
        TODO(TodoStatements.NOT_YET_IMPLEMENTED)
    }

    override fun deletePreviewData(id: String) {
        TODO(TodoStatements.NOT_YET_IMPLEMENTED)
    }

    companion object {
        fun create(): DatabaseVideoDataSource {
            return DatabaseVideoDataSource()
        }
    }
}