package dataclasses.clientrequest.codecs;

import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;

import java.lang.reflect.Type;

public class RequestDecoder implements JsonDeserializer<AnalysisRequest> {


    @Override
    public AnalysisRequest deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        if (json == null) {
            return null;
        }

        String type = json.getAsJsonObject().get("Request").getAsString();
        String auth = json.getAsJsonObject().get("Authorization").getAsString();
        JsonObject body = json.getAsJsonObject().get("Body").getAsJsonObject();
        String pipelineId = body.get("imageId").getAsString();
        String mediaId = body.get("pipelineId").getAsString();

        return new AnalysisRequest(auth, type, mediaId, pipelineId);
    }
}