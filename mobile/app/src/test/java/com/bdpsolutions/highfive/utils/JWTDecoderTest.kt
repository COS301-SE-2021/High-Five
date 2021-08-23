package com.bdpsolutions.highfive.utils

import com.google.common.truth.Truth
import org.junit.Test

class JWTDecoderTest {
    val VALID_JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiZ2l2ZW5fbmFtZSI6InRlc3QiLCJpYXQiOjE1MTYyMzkwMjJ9._DS069AbkNmuA4u9RRAusfgnQoMxc_7J9nsuNDRHbpk"
    val INVALID_JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiaWF0IjoxNTE2MjM5MDIyfQ.L8i6g3PfcHlioHCCPURC9pmXT7gdJpx3kOoyAfNUwCc"

    @Test
    fun `get first name from JWT`() { //NOSONAR
        Truth.assertThat(JWTDecoder.getFirstName(VALID_JWT)).isEqualTo("test")
    }

    @Test
    fun `getting invalid data from JWT returns null`() { //NOSONAR
        Truth.assertThat(JWTDecoder.getFirstName(INVALID_JWT)).isNull()
    }
}