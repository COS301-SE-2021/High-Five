package com.bdpsolutions.highfive.utils.factories

interface RepositoryFactory {
    fun <T> createRepository(classType: Class<T>) : T
}