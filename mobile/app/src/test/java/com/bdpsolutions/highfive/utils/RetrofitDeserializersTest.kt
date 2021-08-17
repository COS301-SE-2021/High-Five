package com.bdpsolutions.highfive.utils

import android.net.Uri
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoInfo
import com.google.common.truth.Truth.assertThat
import com.google.gson.JsonObject
import org.junit.Test
import org.junit.runner.RunWith
import org.mockito.ArgumentMatchers
import org.mockito.ArgumentMatchers.any
import java.text.SimpleDateFormat
import java.util.*
import org.mockito.ArgumentMatchers.anyString
import org.mockito.Mockito.`when`
import org.mockito.Mockito.mock

import org.powermock.api.mockito.PowerMockito
import org.powermock.core.classloader.annotations.PrepareForTest
import org.powermock.modules.junit4.PowerMockRunner


@RunWith(PowerMockRunner::class)
@PrepareForTest(Uri::class)
internal class RetrofitDeserializersTest {

    @Test
    fun `deserialize a VideoPreview object from a JSON object`() { //NOSONAR

        PowerMockito.mockStatic(Uri::class.java)
        val uri: Uri = mock(Uri::class.java)

        PowerMockito.`when`(Uri.parse(any(String::class.java))).thenAnswer {
            return@thenAnswer uri
        }

        val jsonObject = JsonObject()
        jsonObject.addProperty("id", "ID001")
        jsonObject.addProperty("name", "TestOBJ")
        jsonObject.addProperty("url", "http://example.org")
        jsonObject.addProperty("dateStored", "1970-01-01T00:00:00")
        jsonObject.addProperty("thumbnail", "Thumb")

        val expected = VideoInfo(
            id = "ID001",
            name = "TestOBJ",
            url = Uri.parse("http://example.org"),
            dateStored = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()).parse("1970-01-01T00:00:00")!!,
            //thumbnail = "Thumb"
        )

        val obj = RetrofitDeserializers.VideoInfoDeserializer.deserialize(jsonObject, null, null)
        assertThat(obj).isEqualTo(expected)
    }

    @Test
    fun `deserialize an incomplete JSON object`() { //NOSONAR
        val jsonObject = JsonObject()
        jsonObject.addProperty("id", "ID001")
        jsonObject.addProperty("name", "TestOBJ")

        val expected = VideoInfo(
            id = "ID001",
            name = "TestOBJ",
            url = null,
            dateStored = null,
            //thumbnail = null
        )

        val obj = RetrofitDeserializers.VideoInfoDeserializer.deserialize(jsonObject, null, null)
        assertThat(obj).isEqualTo(expected)
    }

    @Test
    fun `deserialize an empty JSON object`() { //NOSONAR
        val jsonObject = JsonObject()
        val expected = VideoInfo(
            id = null,
            name = null,
            url = null,
            dateStored = null,
            //thumbnail = null
        )

        val obj = RetrofitDeserializers.VideoInfoDeserializer.deserialize(jsonObject, null, null)
        assertThat(obj).isEqualTo(expected)
    }
}