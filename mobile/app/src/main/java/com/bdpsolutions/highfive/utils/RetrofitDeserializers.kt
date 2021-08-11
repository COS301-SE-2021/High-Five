package com.bdpsolutions.highfive.utils

import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoPreview
import com.bdpsolutions.highfive.utils.factories.jsonElementConverterFactory
import com.google.gson.*;
import java.lang.reflect.Type
import java.util.*

/**
 * Generic function to convert values in a JsonElement to a desired type.
 *
 * Fetches the item from the Json element by the given name, converts it to the given type
 * and returns the value.
 *
 * If the value does not exist in the Json element, or the type is not supported by the converter
 * factory, the function will return null.
 *
 * @param name The name of the value to fetch
 * @return The desired value or null
 */
inline fun <reified T> JsonElement.getOrNull(name: String): T? {
    return jsonElementConverterFactory(this.asJsonObject[name])
}

object RetrofitDeserializers {
    object VideoPreviewDeserializer : JsonDeserializer<VideoPreview> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): VideoPreview {

            return VideoPreview(
                id = json.getOrNull<String>("id"),
                name = json.getOrNull<String>("name"),
                duration = json.getOrNull<Long>("duration"),
                dateStored = json.getOrNull<Date>("dateStored"),
                thumbnail = json.getOrNull<String>("thumbnail")
            )
        }
    }
}