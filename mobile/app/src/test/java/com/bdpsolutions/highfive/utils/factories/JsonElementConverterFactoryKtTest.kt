package com.bdpsolutions.highfive.utils.factories

import com.google.common.truth.Truth.assertThat
import com.google.gson.JsonObject
import org.junit.Test
import org.junit.runner.RunWith
import org.junit.Before
import org.junit.runners.Suite
import java.util.*

@RunWith(Suite::class)
@Suite.SuiteClasses(
    JsonElementConverterFactoryKtTest.SuccessfulConversions::class,
    JsonElementConverterFactoryKtTest.FailedConversions::class
)
internal class JsonElementConverterFactoryKtTest {

    internal class SuccessfulConversions {
        private lateinit var jsonObject: JsonObject

        @Before
        fun setUp() {
            jsonObject = JsonObject()
            jsonObject.addProperty("string", "ID001")
            jsonObject.addProperty("long", 100)
            jsonObject.addProperty("date", "1970-01-01T00:00:00")
        }

        @Test
        fun `convert Json Element to String`() { //NOSONAR
            assertThat(jsonElementConverterFactory<String>(jsonObject["string"])).isInstanceOf(String::class.java)
        }

        @Test
        fun `convert Json Element to Long`() { //NOSONAR
            assertThat(jsonElementConverterFactory<Long>(jsonObject["long"])).isInstanceOf(java.lang.Long::class.java)
        }

        @Test
        fun `convert Json Element to Date`() { //NOSONAR
            assertThat(jsonElementConverterFactory<Date>(jsonObject["date"])).isInstanceOf(Date::class.java)
        }
    }

    internal class FailedConversions {
        private lateinit var jsonObject: JsonObject

        @Before
        fun setUp() {
            jsonObject = JsonObject()
            jsonObject.addProperty("int", 100)
            jsonObject.addProperty("char", 'c')
        }

        @Test
        fun `converting to this class returns null`() { //NOSONAR
            assertThat(jsonElementConverterFactory<FailedConversions>(jsonObject["int"])).isNull()
        }

        @Test
        fun `accessing non-existent type returns null`() { //NOSONAR
            assertThat(jsonElementConverterFactory<String>(jsonObject["none"])).isNull()
        }
    }
}