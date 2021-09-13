package dataclasses.analysiscommand;

import java.util.UUID;

public class AnalysisCommand {
    private final String mediaType;
    private final String mediaId;
    private final String pipelineId;
    private final String userId;

    public AnalysisCommand(String type, String id, String pipelineId, String userId) {
        mediaType = type;
        mediaId = id;
        this.pipelineId = pipelineId;
        this.userId = userId;
    }

    public String toString() {
        String analysisId = UUID.randomUUID().toString();
        return "{\"CommandId\":\"" + analysisId + "\",\"MediaType\":\"" +
                mediaType + "\",\"MediaId\":\"" + mediaId + "\",\"PipelineId\":\""+ pipelineId + "\",\"UserId\":\"" + userId + "\"}";
    }
}
