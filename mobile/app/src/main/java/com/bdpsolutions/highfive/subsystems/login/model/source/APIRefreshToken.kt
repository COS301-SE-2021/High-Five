package com.bdpsolutions.highfive.subsystems.login.model.source

class APIRefreshToken : LoginDataSource {
    override fun refreshToken() {

    }

    /**
     * Companion object to create the actual class.
     *
     * This is to allow PowerMockito to mock this class when it is created by the
     * ViewModelProviderFactory, by mocking this static method to return a mock
     * class instead of the actual class.
     *
     * The constructor of the parent class is made private to ensure that only this helper
     * object may instantiate the parent class.
     */
    companion object {
        fun create(): APIRefreshToken {
            return APIRefreshToken()
        }
    }
}