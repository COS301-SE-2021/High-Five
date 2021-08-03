package com.bdpsolutions.highfive.utils

import com.bdpsolutions.highfive.models.video.model.VideoPreview
import com.google.gson.*;
import java.lang.reflect.Type
import java.text.SimpleDateFormat
import java.util.*

object RetrofitDeserializers {
    object VideoPreviewDeserializer : JsonDeserializer<VideoPreview> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): VideoPreview {
            val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
            return VideoPreview(
                id = json.asJsonObject["id"].asString,
                name = json.asJsonObject["name"].asString,
                duration = json.asJsonObject["duration"].asLong,
                dateStored = format.parse(json.asJsonObject["dateStored"].asString)!!,
                thumbnail = json.asJsonObject["thumbnail"].asString
            )
        }
    }
}