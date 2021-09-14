package dataclasses.analysiscommand.commandbody;

public class LiveAnalysisCommandBody implements CommandBody {
    private final String streamId;
    private final String publishLink;

    public LiveAnalysisCommandBody(String streamId, String publishLink) {
        this.streamId = streamId;
        this.publishLink = publishLink;
    }

    public String getPublishLink() {
        return publishLink;
    }

    public String getStreamId() {
        return streamId;
    }

    public String toString() {
        return "{\"StreamId\":\"" + streamId + "\",\"PublishLink\":\"" + publishLink + "\"}";
    }
}
