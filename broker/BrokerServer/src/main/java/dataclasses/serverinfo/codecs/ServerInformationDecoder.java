package dataclasses.serverinfo.codecs;

import com.google.gson.*;
import dataclasses.serverinfo.ServerInformation;

import java.lang.reflect.Type;

public class ServerInformationDecoder implements JsonDeserializer<ServerInformation> {

    /**
     * Deserializes a JSON object into a ServerInformation object. The usage information is not decoded,
     * as this is done by the TelemetryBuilder.
     *
     * @param json JSON object to deserialize
     * @param typeOfT
     * @param context
     * @return the deserialized object (without usage information)
     * @see dataclasses.telemetry.builder.TelemetryBuilder
     */
    @Override
    public ServerInformation deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        String id = json.getAsJsonObject().get("ServerId").getAsString();
        return new ServerInformation(id);
    }
}