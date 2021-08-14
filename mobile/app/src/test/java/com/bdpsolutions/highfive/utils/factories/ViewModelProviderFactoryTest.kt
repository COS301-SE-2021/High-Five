package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.helpers.TestViewModel
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.bdpsolutions.highfive.subsystems.video.model.VideoDataRepository
import com.bdpsolutions.highfive.subsystems.video.model.source.APIVideoDataSource
import com.bdpsolutions.highfive.subsystems.video.model.source.DatabaseVideoDataSource
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
    LoginViewModel.Companion::class,
    LoginRepository.Companion::class,
    APIVideoDataSource.Companion::class,
    DatabaseVideoDataSource.Companion::class,
    VideoDataRepository.Companion::class,
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
            val repoMock = mock(LoginRepository::class.java)
            val repoCompanionMock = mock(LoginRepository.Companion::class.java)
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
            PowerMockito.mockStatic(LoginRepository.Companion::class.java)
            PowerMockito.whenNew(LoginRepository.Companion::class.java)
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

            //create mock DatabaseVideoDataSource class
            val dbMock = mock(DatabaseVideoDataSource::class.java)
            val dbCompanionMock = mock(DatabaseVideoDataSource.Companion::class.java)
            `when`(dbCompanionMock.create()).thenReturn(dbMock)

            //create mock VideoDataRepository
            val repoMock = mock(VideoDataRepository::class.java)
            val repoCompanionMock = mock(VideoDataRepository.Companion::class.java)
            `when`(repoCompanionMock.create(apiCompanionMock.create(), dbCompanionMock.create()))
                .thenReturn(repoMock)

            //create mock VideoViewModel
            val vmMock = mock(VideoViewModel::class.java)
            val vmCompanionMock = mock(VideoViewModel.Companion::class.java)
            `when`(vmCompanionMock
                .create(
                    repoCompanionMock.create(apiCompanionMock.create(), dbCompanionMock.create())
                ))
                .thenReturn(vmMock)

            //Mock data sources
            PowerMockito.mockStatic(APIVideoDataSource.Companion::class.java)
            PowerMockito.whenNew(APIVideoDataSource.Companion::class.java)
                .withNoArguments()
                .thenReturn(apiCompanionMock)

            PowerMockito.mockStatic(DatabaseVideoDataSource.Companion::class.java)
            PowerMockito.whenNew(DatabaseVideoDataSource.Companion::class.java)
                .withNoArguments()
                .thenReturn(dbCompanionMock)


            //Mock repository
            PowerMockito.mockStatic(VideoDataRepository.Companion::class.java)
            PowerMockito.whenNew(VideoDataRepository.Companion::class.java)
                .withNoArguments()
                .thenReturn(repoCompanionMock)


            //Mock view model
            PowerMockito.mockStatic(VideoViewModel.Companion::class.java)
            PowerMockito.whenNew(VideoViewModel.Companion::class.java)
                .withNoArguments()
                .thenReturn(vmCompanionMock)
        }
        ////////////////////////////////////////////////////////////////////////////////////////////
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
        val viewModel = factory.create(VideoViewModel::class.java)
        Truth.assertThat(viewModel).isInstanceOf(VideoViewModel::class.java)
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