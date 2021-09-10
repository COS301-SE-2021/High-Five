package dataclasses.serverinfo.codecs;

import com.google.gson.*;
import dataclasses.serverinfo.ServerUsage;

import java.lang.reflect.Type;

public class ServerUsageDecoder implements JsonDeserializer<ServerUsage> {
    @Override
    public ServerUsage deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        JsonObject usageBody = json.getAsJsonObject().get("Usage").getAsJsonObject();

        short cpu = usageBody.get("CpuUsage").getAsShort();
        short gpu = usageBody.get("GpuUsage").getAsShort();
        short net = usageBody.get("NetUsage").getAsShort();
        short disk = usageBody.get("DiskUsage").getAsShort();

        ServerUsage usage = new ServerUsage();
        usage.cpu = cpu;
        usage.gpu = gpu;
        usage.disk = disk;
        usage.net = net;

        return usage;
    }
}
