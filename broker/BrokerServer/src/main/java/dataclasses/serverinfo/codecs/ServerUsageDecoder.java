package dataclasses.serverinfo.codecs;

import com.google.gson.*;
import dataclasses.serverinfo.ServerUsage;

import java.lang.reflect.Type;

public class ServerUsageDecoder implements JsonDeserializer<ServerUsage> {
    @Override
    public ServerUsage deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        JsonObject usageBody = json.getAsJsonObject().get("usage").getAsJsonObject();

        short cpu = usageBody.get("cpu").getAsShort();
        short gpu = usageBody.get("gpu").getAsShort();
        short net = usageBody.get("net").getAsShort();
        short disk = usageBody.get("disk").getAsShort();

        ServerUsage usage = new ServerUsage();
        usage.cpu = cpu;
        usage.gpu = gpu;
        usage.disk = disk;
        usage.net = net;

        return usage;
    }
}
