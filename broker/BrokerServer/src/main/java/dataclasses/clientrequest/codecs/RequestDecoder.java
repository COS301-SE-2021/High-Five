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
        JsonElement possibleBody = json.getAsJsonObject().get("Body");
        if (possibleBody.isJsonNull() && type.contains("Syn")) {
            return new AnalysisRequest(auth, type, userId, null);
        }
        JsonObject body = possibleBody.getAsJsonObject();
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
            String publishLinkDrone = body.get("PublishLinkDrone").getAsString();
            String playLinkWeb;
            String publishLinkAnalysisEngine;
            String playLinkAnalysisEngine;
            try {
                playLinkWeb = body.get("PlayLinkWeb").getAsString();
            } catch (Exception ignored) {
                playLinkWeb = "none";
            }
            try {
                publishLinkAnalysisEngine = body.get("PublishLinkAnalysisEngine").getAsString();
            } catch (Exception ignored) {
                publishLinkAnalysisEngine = "none";
            }
            try {
                playLinkAnalysisEngine = body.get("PlayLinkAnalysisEngine").getAsString();
            } catch (Exception ignored) {
                playLinkAnalysisEngine = "none";
            }

            String streamId = body.get("StreamId").getAsString();
            requestBody = new LiveAnalysisRequestBody(playLinkWeb, publishLinkDrone, playLinkAnalysisEngine, publishLinkAnalysisEngine, streamId);
        } else {
            String mediaId = body.get(mediaIdName).getAsString();
            String pipelineId = body.get("pipelineId").getAsString();
            requestBody = new StoredMediaRequestBody(mediaId, pipelineId);
        }

        return new AnalysisRequest(auth, type, userId, requestBody);
    }
}