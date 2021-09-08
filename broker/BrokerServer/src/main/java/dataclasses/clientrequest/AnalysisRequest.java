package dataclasses.clientrequest;


public class AnalysisRequest {
    private final String authorization;
    private final String requestType;
    private final String mediaId;
    private final String pipelineId;

    public AnalysisRequest(String authorization, String requestType, String mediaId, String pipelineId) {
        this.authorization = authorization;
        this.requestType = requestType;
        this.mediaId = mediaId;
        this.pipelineId = pipelineId;
    }

    public String getAuthorization() {
        return authorization;
    }

    public String getRequestType() {
        return requestType;
    }

    public String getMediaId() {
        return mediaId;
    }

    public String getPipelineId() {
        return pipelineId;
    }
}
