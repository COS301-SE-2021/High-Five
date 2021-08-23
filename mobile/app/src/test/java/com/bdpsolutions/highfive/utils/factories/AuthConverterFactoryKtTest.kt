package com.bdpsolutions.highfive.utils.factories

import com.bdpsolutions.highfive.constants.Tests.AUTH_CONVERTER_FACORY as auth
import com.bdpsolutions.highfive.constants.Exceptions.AUTH_CONVERTER_FACTORY as authExcept

import android.os.Bundle
import android.text.TextUtils
import com.google.common.truth.Truth
import org.junit.Before

import org.junit.Test
import org.junit.runner.RunWith

import org.mockito.Mockito.*
import org.mockito.Mockito.`when`
import org.powermock.core.classloader.annotations.PrepareForTest
import org.powermock.modules.junit4.PowerMockRunner

import org.powermock.api.mockito.PowerMockito
import java.lang.Exception


@RunWith(PowerMockRunner::class)
@PrepareForTest(TextUtils::class)
internal class AuthConverterFactoryKtTest {

    var mBundleMock: Bundle? = null

    init {
        mBundleMock = mock(Bundle::class.java)

        `when`(mBundleMock?.getString(auth.STRING_KEY)).thenReturn(auth.STRING_VAL)
        `when`(mBundleMock?.getBoolean(auth.BOOL_KEY)).thenReturn(auth.BOOL_VAL)
        `when`(mBundleMock?.getString(auth.INVALID_KEY)).thenReturn(auth.INVALID_VAL)

        PowerMockito.mockStatic(TextUtils::class.java)
        PowerMockito.`when`(TextUtils.isEmpty(any(CharSequence::class.java)))
            .thenAnswer { invocation ->
                val a = invocation.arguments[0] as CharSequence
                return@thenAnswer a.isEmpty()
            }
    }

    @Test
    fun `fetch a string from Bundle`() { //NOSONAR
        Truth.assertThat(authConverterFactory<String>(auth.STRING_KEY, mBundleMock!!)).isEqualTo(auth.STRING_VAL)
    }

    @Test
    fun `fetch a boolean from Bundle`() { //NOSONAR
        Truth.assertThat(authConverterFactory<Boolean>(auth.BOOL_KEY, mBundleMock!!)).isEqualTo(auth.BOOL_VAL)
    }

    @Test
    fun `try fetch string value not in bundle`() { //NOSONAR
        try {
            authConverterFactory<String>(auth.INVALID_KEY, mBundleMock!!)
        } catch (e: Exception) {
            Truth.assertThat(e.message).isEqualTo(authExcept.NO_SUCH_VALUE)
        }
    }

    @Test
    fun `try fetch bool value not in bundle`() { //NOSONAR
        try {
            authConverterFactory<Boolean>(auth.INVALID_KEY, mBundleMock!!)
        } catch (e: Exception) {
            Truth.assertThat(e.message).isEqualTo(authExcept.NO_SUCH_VALUE)
        }
    }

    @Test
    fun `try fetch unsupported class from bundle`() { //NOSONAR
        try {
            authConverterFactory<Float>(auth.INVALID_KEY, mBundleMock!!)
        } catch (e: Exception) {
            Truth.assertThat(e.message).isEqualTo(authExcept.UNSUPPORTED_CLASS)
        }
    }
}