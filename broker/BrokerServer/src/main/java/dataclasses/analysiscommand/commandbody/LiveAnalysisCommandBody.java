package dataclasses.analysiscommand.commandbody;

import com.google.gson.Gson;
import com.google.gson.annotations.SerializedName;

public class LiveAnalysisCommandBody implements CommandBody {
    @SerializedName("PlayLink") String playLink;

    public LiveAnalysisCommandBody(String publishLink) {
        this.playLink = publishLink;
    }

    public String toString() {
        return new Gson().toJson(this);
    }

}
