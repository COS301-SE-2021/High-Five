package dataclasses.analysiscommand.commandbody;

public class StoredMediaCommandBody implements CommandBody {
    private final String mediaId;
    private final String pipelineId;

    public StoredMediaCommandBody(String mediaId, String pipelineId) {
        this.mediaId = mediaId;
        this.pipelineId = pipelineId;
    }

    public String getMediaId() {
        return mediaId;
    }

    public String getPipelineId() {
        return pipelineId;
    }

    public String toString() {
        return "{\"PipelineId\":\"" + pipelineId + "\",\"MediaId\":\"" + mediaId + "\"}";
    }
}
