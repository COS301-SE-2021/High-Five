package com.bdpsolutions.highfive.utils

import android.net.Uri
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageInfo
import com.bdpsolutions.highfive.subsystems.image.model.dataclass.ImageUploadResult
import com.bdpsolutions.highfive.subsystems.login.model.dataclass.AccessTokenResponse
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoInfo
import com.bdpsolutions.highfive.subsystems.video.model.dataclass.VideoUploadResult
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
    object VideoInfoDeserializer : JsonDeserializer<VideoInfo> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): VideoInfo {

            return VideoInfo(
                id = json.getOrNull<String>("id"),
                name = json.getOrNull<String>("name"),
                url = json.getOrNull<Uri>("url"),
                dateStored = json.getOrNull<Date>("dateStored"),
                //thumbnail = json.getOrNull<String>("thumbnail")
            )
        }
    }

    object AccessTokenDeserializer : JsonDeserializer<AccessTokenResponse> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): AccessTokenResponse {

            return AccessTokenResponse(
                scope = json.getOrNull<String>("scope"),
                tokenExpires = json.getOrNull<Int>("id_token_expires_in"),
                refreshExpires = json.getOrNull<Int>("refresh_token_expires_in"),
                idToken = json.getOrNull<String>("id_token"),
                refreshToken = json.getOrNull<String>("refresh_token")
            )
        }
    }

    object ImageInfoDeserializer : JsonDeserializer<ImageInfo> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): ImageInfo {

            return ImageInfo(
                id = json.getOrNull<String>("id"),
                name = json.getOrNull<String>("name"),
                dateStored = json.getOrNull<Date>("dateStored"),
                url = json.getOrNull<Uri>("url")
            )
        }
    }

    object ImageUploadResultDeserializer : JsonDeserializer<ImageUploadResult> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): ImageUploadResult {
            return ImageUploadResult(
                success = json.getOrNull<Boolean>("success"),
                imageId = json.getOrNull<String>("imageId")
            )
        }

    }

    object VideoUploadResultDeserializer : JsonDeserializer<VideoUploadResult> {
        override fun deserialize(
            json: JsonElement,
            typeOfT: Type?,
            context: JsonDeserializationContext?
        ): VideoUploadResult {
            return VideoUploadResult(
                success = json.getOrNull<Boolean>("success"),
                videoId = json.getOrNull<String>("videoId")
            )
        }

    }
}