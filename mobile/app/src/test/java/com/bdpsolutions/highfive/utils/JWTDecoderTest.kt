package com.bdpsolutions.highfive.utils

import android.net.Uri
import android.util.Base64
import com.google.common.truth.Truth
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers
import org.mockito.Mockito
import org.powermock.api.mockito.PowerMockito
import org.powermock.core.classloader.annotations.PrepareForTest
import org.powermock.modules.junit4.PowerMockRunner
import java.nio.charset.Charset

@RunWith(PowerMockRunner::class)
@PrepareForTest(Base64::class)
class JWTDecoderTest {
    val VALID_JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiZ2l2ZW5fbmFtZSI6InRlc3QiLCJpYXQiOjE1MTYyMzkwMjJ9._DS069AbkNmuA4u9RRAusfgnQoMxc_7J9nsuNDRHbpk"
    val INVALID_JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiaWF0IjoxNTE2MjM5MDIyfQ.L8i6g3PfcHlioHCCPURC9pmXT7gdJpx3kOoyAfNUwCc"

    @Test
    fun `get first name from JWT`() { //NOSONAR
        PowerMockito.mockStatic(Base64::class.java)

        PowerMockito.`when`(Base64.decode(ArgumentMatchers.any(String::class.java),ArgumentMatchers.any(Int::class.java))).thenAnswer {
            return@thenAnswer "\"given_name\":\"test\"".toByteArray(Charset.defaultCharset())
        }
        Truth.assertThat(JWTDecoder.getFirstName(VALID_JWT)).isEqualTo("test")
    }

    @Test
    fun `getting invalid data from JWT returns null`() { //NOSONAR
        PowerMockito.mockStatic(Base64::class.java)

        PowerMockito.`when`(Base64.decode(ArgumentMatchers.any(String::class.java),ArgumentMatchers.any(Int::class.java))).thenAnswer {
            return@thenAnswer "\"fail\":\"test\"".toByteArray(Charset.defaultCharset())
        }
        Truth.assertThat(JWTDecoder.getFirstName(INVALID_JWT)).isNull()
    }
}