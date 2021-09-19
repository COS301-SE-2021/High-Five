package dataclasses.serverinfo.codecs;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;
import dataclasses.serverinfo.ServerRegistrationInfo;

import java.lang.reflect.Type;

public class ServerRegistrationInfoDecoder implements JsonDeserializer<ServerRegistrationInfo> {
    @Override
    public ServerRegistrationInfo deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        String id = json.getAsJsonObject().get("ServerId").getAsString();
        long timestamp = json.getAsJsonObject().get("Timestamp").getAsLong();
        return new ServerRegistrationInfo(id, timestamp);
    }
}
