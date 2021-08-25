package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepositoryImpl
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import java.lang.Exception
import javax.inject.Inject

class RepositoryFactoryImpl @Inject constructor() : RepositoryFactory {

    @Suppress("UNCHECKED_CAST")
    override fun <T> createRepository(classType: Class<T>) : T {
        return when (classType) {
            VideoDataRepository::class -> VideoDataRepositoryImpl.create(APIVideoDataSource.create())
            else -> throw Exception("Unknown class type ${classType.canonicalName}")
        } as T
    }
}

