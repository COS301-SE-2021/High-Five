package com.bdpsolutions.highfive.utils.factories

import android.os.Bundle
import android.text.TextUtils
import androidx.lifecycle.ViewModel
import com.bdpsolutions.highfive.subsystems.login.model.LoginRepository
import com.bdpsolutions.highfive.subsystems.login.model.source.APILogin
import com.bdpsolutions.highfive.subsystems.login.model.source.LoginDataSource
import com.bdpsolutions.highfive.subsystems.login.viewmodel.LoginViewModel
import com.google.common.truth.Truth
import org.junit.Assert.*

import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.Mockito
import org.mockito.Mockito.*
import org.mockito.Mockito.`when`
import org.powermock.api.mockito.PowerMockito
import org.powermock.core.classloader.annotations.PrepareForTest
import org.powermock.modules.junit4.PowerMockRunner

@RunWith(PowerMockRunner::class)
@PrepareForTest(
    APILogin.Companion::class,
    LoginViewModel.Companion::class,
    LoginRepository.Companion::class
)
class ViewModelProviderFactoryTest {

    init {

        //Create mock objects for the LoginViewModel class
        ///////////////////////////////////////////////////////////////////////////////////////////

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
        ////////////////////////////////////////////////////////////////////////////////////////////
    }

    @Test
    fun `create LoginViewModel class from factory`() { //NOSONAR
        val factory = ViewModelProviderFactory()
        val viewModel = factory.create(LoginViewModel::class.java)
        Truth.assertThat(viewModel).isInstanceOf(LoginViewModel::class.java)
    }
}