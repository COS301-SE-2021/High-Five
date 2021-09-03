package dataclasses.serverinfo.codecs;

import com.google.gson.*;
import dataclasses.serverinfo.ServerInformation;

import java.lang.reflect.Type;

public class ServerInformationDecoder implements JsonDeserializer<ServerInformation> {

    @Override
    public ServerInformation deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        String address = json.getAsJsonObject().get("address").getAsString();
        String id = json.getAsJsonObject().get("serverId").getAsString();
        String port = json.getAsJsonObject().get("port").getAsString();
        String credentials = json.getAsJsonObject().get("credentials").getAsString();
        return new ServerInformation(address, id, port, credentials);
    }
}