package dataclasses.clientrequest.codecs;

import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;

import java.lang.reflect.Type;

public class RequestDecoder implements JsonDeserializer<AnalysisRequest> {


    @Override
    public AnalysisRequest deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        String type = json.getAsJsonObject().get("analysis_type").getAsString();
        String auth = json.getAsJsonObject().get("auth_challenge").getAsString();
        String mediaType = json.getAsJsonObject().get("media_type").getAsString();
        String mediaId = json.getAsJsonObject().get("media_id").getAsString();

        return new AnalysisRequest(type, auth, mediaType, mediaId);
    }
}