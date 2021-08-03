package com.bdpsolutions.highfive.utils

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.google.common.truth.Truth.assertThat
import com.google.gson.JsonObject
import org.junit.Test
import java.text.SimpleDateFormat
import java.util.*

internal class RetrofitDeserializersTest {

    @Test
    fun `deserialize a VideoPreview object from a JSON object`() {

        val jsonObject = JsonObject()
        jsonObject.addProperty("id", "ID001")
        jsonObject.addProperty("name", "TestOBJ")
        jsonObject.addProperty("duration", 100)
        jsonObject.addProperty("dateStored", "1970-01-01T00:00:00")
        jsonObject.addProperty("thumbnail", "Thumb")

        val expected = VideoPreview(
            id = "ID001",
            name = "TestOBJ",
            duration = 100,
            dateStored = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()).parse("1970-01-01T00:00:00")!!,
            thumbnail = "Thumb"
        )

        val obj = RetrofitDeserializers.VideoPreviewDeserializer.deserialize(jsonObject, null, null)
        assertThat(obj).isEqualTo(expected)
    }
}