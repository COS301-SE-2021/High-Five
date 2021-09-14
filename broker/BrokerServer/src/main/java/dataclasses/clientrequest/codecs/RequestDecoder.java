package dataclasses.clientrequest.codecs;

import com.google.gson.*;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.requestbody.LiveAnalysisRequestBody;
import dataclasses.clientrequest.requestbody.RequestBody;
import dataclasses.clientrequest.requestbody.StoredMediaRequestBody;

import java.lang.reflect.Type;

public class RequestDecoder implements JsonDeserializer<AnalysisRequest> {


    @Override
    public AnalysisRequest deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        if (json == null) {
            return null;
        }

        String type = json.getAsJsonObject().get("Request").getAsString();
        String auth = json.getAsJsonObject().get("Authorization").getAsString();
        String userId = json.getAsJsonObject().get("UserId").getAsString();
        JsonObject body = json.getAsJsonObject().get("Body").getAsJsonObject();
        String mediaIdName;
        RequestBody requestBody;
        if (type.equals("AnalyzeImage")) {
            mediaIdName = "imageId";
        } else if (type.equals("AnalyzeVideo")) {
            mediaIdName = "videoId";
        } else {
            mediaIdName = null;
        }

        if (mediaIdName == null) {
            String publishLink = body.get("PublishLink").getAsString();
            String playLink = body.get("PlayLink").getAsString();
            String streamId = body.get("StreamId").getAsString();
            requestBody = new LiveAnalysisRequestBody(playLink, publishLink, streamId);
        } else {
            String mediaId = body.get(mediaIdName).getAsString();
            String pipelineId = body.get("pipelineId").getAsString();
            requestBody = new StoredMediaRequestBody(mediaId, pipelineId);
        }

        return new AnalysisRequest(auth, type, userId, requestBody);
    }
}