package com.bdpsolutions.highfive.utils

import android.net.Uri
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoInfo
import com.google.common.truth.Truth.assertThat
import com.google.gson.JsonObject
import org.junit.Test
import java.text.SimpleDateFormat
import java.util.*

internal class RetrofitDeserializersTest {

    @Test
    fun `deserialize a VideoPreview object from a JSON object`() { //NOSONAR

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