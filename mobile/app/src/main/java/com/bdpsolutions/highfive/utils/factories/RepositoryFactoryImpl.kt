package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.constants.RepositoryTypes
import com.bdpsolutions.highfive.subsystems.image.model.repository.ImageRepositoryImpl
import com.bdpsolutions.highfive.subsystems.image.model.source.APIImageDataSource
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepositoryImpl
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import javax.inject.Inject

class RepositoryFactoryImpl @Inject constructor() : RepositoryFactory {

    @Suppress("UNCHECKED_CAST")
    override fun createRepository(classType: RepositoryTypes) : Any {

        return repoInstances.getOrPut(classType, {
            when (classType) {
                RepositoryTypes.VIDEO_REPOSITORY -> VideoDataRepositoryImpl.create(APIVideoDataSource.create())
                RepositoryTypes.IMAGE_REPOSITORY -> ImageRepositoryImpl.create(APIImageDataSource.create())
            }
        })
    }

    companion object {
        private val repoInstances: HashMap<Any, Any> = HashMap()
    }
}

