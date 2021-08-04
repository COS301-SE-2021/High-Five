package com.bdpsolutions.highfive

import com.bdpsolutions.highfive.utils.RetrofitDeserializersTest
import com.bdpsolutions.highfive.utils.factories.JsonElementConverterFactoryKtTest
import org.junit.runner.RunWith
import org.junit.runners.Suite

@RunWith(Suite::class)
@Suite.SuiteClasses(
    RetrofitDeserializersTest::class,
    JsonElementConverterFactoryKtTest::class
)
class UnitTests {
}