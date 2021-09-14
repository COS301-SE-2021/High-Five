package dataclasses.clientrequest.requestbody;

public class StoredMediaRequestBody implements RequestBody {
    private final String mediaId;
    private final String pipelineId;

    public StoredMediaRequestBody(String mediaId, String pipelineId) {
        this.mediaId = mediaId;
        this.pipelineId = pipelineId;
    }

    public String getPipelineId() {
        return pipelineId;
    }

    public String getMediaId() {
        return mediaId;
    }
}
