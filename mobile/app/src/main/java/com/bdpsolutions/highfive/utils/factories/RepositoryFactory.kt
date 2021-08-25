package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.constants.RepositoryTypes

interface RepositoryFactory {
    fun createRepository(classType: RepositoryTypes) : Any
}