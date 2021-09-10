package dataclasses.analysiscommand;

import java.util.UUID;

public class AnalysisCommand {
    private final String mediaType;
    private final String mediaId;
    private final String pipelineId;

    public AnalysisCommand(String type, String id, String pipelineId) {
        mediaType = type;
        mediaId = id;
        this.pipelineId = pipelineId;
    }

    public String toString() {
        String analysisId = UUID.randomUUID().toString();
        return "{\"commandId\": \"" + analysisId + "\", \"analyze\": { \"mediatype\": \"" +
                mediaType + "\", \"mediaId\": \"" + mediaId + "\", \"pipelineId\": \""+ pipelineId + "\"}}";
    }
}
