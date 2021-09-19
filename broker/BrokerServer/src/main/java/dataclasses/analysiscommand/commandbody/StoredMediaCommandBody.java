package dataclasses.analysiscommand.commandbody;

import com.google.gson.Gson;
import com.google.gson.annotations.SerializedName;

public class StoredMediaCommandBody implements CommandBody {
    @SerializedName("MediaId") String mediaId;
    @SerializedName("PipelineId") String pipelineId;

    public StoredMediaCommandBody(String mediaId, String pipelineId) {
        this.mediaId = mediaId;
        this.pipelineId = pipelineId;
    }

    public String toString() {
        return new Gson().toJson(this);
    }
}
