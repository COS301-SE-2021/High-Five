package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.helpers.TestViewModel
import com.bdpsolutions.highfive.subsystems.login.model.AuthenticationRepositoryImpl
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.login.model.source.APIRefreshToken
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.splash.viewmodel.SplashViewModel
import com.bdpsolutions.highfive.subsystems.video.model.repository.VideoDataRepositoryImpl
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.video.viewmodel.VideoViewModel
import com.bdpsolutions.highfive.constants.Exceptions.VIEWMODEL_PROVIDER_FACTORY as vmf
import com.google.common.truth.Truth

import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.Mockito.*
import org.mockito.Mockito.`when`
import org.powermock.api.mockito.PowerMockito
import org.powermock.core.classloader.annotations.PrepareForTest
import org.powermock.modules.junit4.PowerMockRunner

@RunWith(PowerMockRunner::class)
@PrepareForTest(
    APILogin.Companion::class,
    APIRefreshToken.Companion::class,
    LoginViewModel.Companion::class,
    SplashViewModel.Companion::class,
    AuthenticationRepositoryImpl.Companion::class,
    APIVideoDataSource.Companion::class,
    VideoDataRepositoryImpl.Companion::class,
    VideoViewModel.Companion::class
)
class ViewModelProviderFactoryTest {

    init {

        //Create mock objects for the LoginViewModel class
        ///////////////////////////////////////////////////////////////////////////////////////////
        run {

            //create mock APILogin class
            val apiMock = mock(APILogin::class.java)
            val apiCompanionMock = mock(APILogin.Companion::class.java)
            `when`(apiCompanionMock.create()).thenReturn(apiMock)

            //create mock LoginRepository
            val repoMock = mock(AuthenticationRepositoryImpl::class.java)
            val repoCompanionMock = mock(AuthenticationRepositoryImpl.Companion::class.java)
            `when`(repoCompanionMock.create(apiCompanionMock.create())).thenReturn(repoMock)

            //create mock LoginViewModel
            val vmMock = mock(LoginViewModel::class.java)
            val vmCompanionMock = mock(LoginViewModel.Companion::class.java)
            `when`(vmCompanionMock.create(repoCompanionMock.create(APILogin.create())))
                .thenReturn(vmMock)

            //Mock data source
            PowerMockito.mockStatic(APILogin.Companion::class.java)
            PowerMockito.whenNew(APILogin.Companion::class.java)
                .withNoArguments()
                .thenReturn(apiCompanionMock)


            //Mock repository
            PowerMockito.mockStatic(AuthenticationRepositoryImpl.Companion::class.java)
            PowerMockito.whenNew(AuthenticationRepositoryImpl.Companion::class.java)
                .withNoArguments()
                .thenReturn(repoCompanionMock)


            //Mock view model
            PowerMockito.mockStatic(LoginViewModel.Companion::class.java)
            PowerMockito.whenNew(LoginViewModel.Companion::class.java)
                .withNoArguments()
                .thenReturn(vmCompanionMock)
        }
        ////////////////////////////////////////////////////////////////////////////////////////////

        //Create mock objects for VideoViewModel class
        ////////////////////////////////////////////////////////////////////////////////////////////
        run {
            //create mock APIVideoDataSource class
            val apiMock = mock(APIVideoDataSource::class.java)
            val apiCompanionMock = mock(APIVideoDataSource.Companion::class.java)
            `when`(apiCompanionMock.create()).thenReturn(apiMock)


            //create mock VideoDataRepository
            val repoMock = mock(VideoDataRepositoryImpl::class.java)
            val repoCompanionMock = mock(VideoDataRepositoryImpl.Companion::class.java)
            `when`(repoCompanionMock.create(apiCompanionMock.create()))
                .thenReturn(repoMock)

            //create mock VideoViewModel
            val vmMock = mock(VideoViewModel::class.java)
            val vmCompanionMock = mock(VideoViewModel.Companion::class.java)
            `when`(vmCompanionMock
                .create(
                    repoCompanionMock.create(apiCompanionMock.create())
                ))
                .thenReturn(vmMock)

            //Mock data sources
            PowerMockito.mockStatic(APIVideoDataSource.Companion::class.java)
            PowerMockito.whenNew(APIVideoDataSource.Companion::class.java)
                .withNoArguments()
                .thenReturn(apiCompanionMock)


            //Mock repository
            PowerMockito.mockStatic(VideoDataRepositoryImpl.Companion::class.java)
            PowerMockito.whenNew(VideoDataRepositoryImpl.Companion::class.java)
                .withNoArguments()
                .thenReturn(repoCompanionMock)


            //Mock view model
            PowerMockito.mockStatic(VideoViewModel.Companion::class.java)
            PowerMockito.whenNew(VideoViewModel.Companion::class.java)
                .withNoArguments()
                .thenReturn(vmCompanionMock)
        }
        ////////////////////////////////////////////////////////////////////////////////////////////

        //Create mock objects for the SplashViewModel class
        ////////////////////////////////////////////////////////////////////////////////////////////
        run {
            //create mock APILogin class
            val apiMock = mock(APIRefreshToken::class.java)
            val apiCompanionMock = mock(APIRefreshToken.Companion::class.java)
            `when`(apiCompanionMock.create()).thenReturn(apiMock)

            //create mock LoginRepository
            val repoMock = mock(AuthenticationRepositoryImpl::class.java)
            val repoCompanionMock = mock(AuthenticationRepositoryImpl.Companion::class.java)
            `when`(repoCompanionMock.create(apiCompanionMock.create())).thenReturn(repoMock)

            //create mock LoginViewModel
            val vmMock = mock(SplashViewModel::class.java)
            val vmCompanionMock = mock(SplashViewModel.Companion::class.java)
            `when`(vmCompanionMock.create(repoCompanionMock.create(APILogin.create())))
                .thenReturn(vmMock)

            //Mock data source
            PowerMockito.mockStatic(APIRefreshToken.Companion::class.java)
            PowerMockito.whenNew(APIRefreshToken.Companion::class.java)
                .withNoArguments()
                .thenReturn(apiCompanionMock)


            //Mock repository
            PowerMockito.mockStatic(AuthenticationRepositoryImpl.Companion::class.java)
            PowerMockito.whenNew(AuthenticationRepositoryImpl.Companion::class.java)
                .withNoArguments()
                .thenReturn(repoCompanionMock)


            //Mock view model
            PowerMockito.mockStatic(SplashViewModel.Companion::class.java)
            PowerMockito.whenNew(SplashViewModel.Companion::class.java)
                .withNoArguments()
                .thenReturn(vmCompanionMock)
        }
    }

    @Test
    fun `create LoginViewModel class from factory`() { //NOSONAR
        val factory = ViewModelProviderFactory()
        val viewModel = factory.create(LoginViewModel::class.java)
        Truth.assertThat(viewModel).isInstanceOf(LoginViewModel::class.java)
    }

    @Test
    fun `create VideoViewModel class from factory`() { //NOSONAR
        val factory = ViewModelProviderFactory()
        val repositoryFactory = RepositoryFactoryImpl()
        factory.repoFactory = repositoryFactory
        val viewModel = factory.create(VideoViewModel::class.java)
        Truth.assertThat(viewModel).isInstanceOf(VideoViewModel::class.java)
    }

    @Test
    fun `create SplashViewModel class from factory`() { //NOSONAR
        val factory = ViewModelProviderFactory()
        val viewModel = factory.create(SplashViewModel::class.java)
        Truth.assertThat(viewModel).isInstanceOf(SplashViewModel::class.java)
    }

    @Test
    fun `Try create invalid ViewModel`() { //NOSONAR
        try {
            val factory = ViewModelProviderFactory()
            factory.create(TestViewModel::class.java)
        } catch (e: IllegalArgumentException) {
            Truth.assertThat(e.message).isEqualTo(vmf.UNKNOWN_VIEWMODEL)
        }
    }
}