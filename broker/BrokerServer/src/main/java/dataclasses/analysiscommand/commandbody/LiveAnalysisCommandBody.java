package dataclasses.analysiscommand.commandbody;

import com.google.gson.Gson;
import com.google.gson.annotations.SerializedName;

public class LiveAnalysisCommandBody implements CommandBody {
    @SerializedName("StreamId") String streamId;
    @SerializedName("PublishLink") String publishLink;
    @SerializedName("PlayLink") String playLink;

    public LiveAnalysisCommandBody(String streamId, String publishLink, String playLink) {
        this.streamId = streamId;
        this.publishLink = publishLink;
        this.playLink = playLink;
    }

    public String toString() {
        return new Gson().toJson(this);
    }

}
