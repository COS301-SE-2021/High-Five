package com.bdpsolutions.highfive

import com.bdpsolutions.highfive.utils.RetrofitDeserializersTest
import com.bdpsolutions.highfive.utils.factories.JsonElementConverterFactoryKtTest
import org.junit.runner.RunWith
import org.junit.runners.Suite

//IMPORTANT: the //NOSONAR comments are to disable code smell checking for these unit test
// functions. This is due to the naming convention of the unit tests, which SonarCloud does not like.
@RunWith(Suite::class)
@Suite.SuiteClasses(
    RetrofitDeserializersTest::class,
    JsonElementConverterFactoryKtTest::class
)
class UnitTests {
}