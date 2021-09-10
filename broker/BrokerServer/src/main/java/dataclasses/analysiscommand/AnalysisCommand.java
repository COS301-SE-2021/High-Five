package dataclasses.analysiscommand;

public class AnalysisCommand {
    private final String mediaType;
    private final String mediaId;
    private final String pipelineId;
    private final String topic;

    public AnalysisCommand(String type, String id, String pipelineId, String topic) {
        mediaType = type;
        mediaId = id;
        this.pipelineId = pipelineId;
        this.topic = topic;
    }

    public String toString() {
        return "{\"topic\": \"" + topic + "\", \"analyze\": { \"mediatype\": \"" +
                mediaType + "\", \"mediaId\": " + mediaId + "\", \"pipelineId\": \""+ pipelineId + "\"}}";
    }
}
