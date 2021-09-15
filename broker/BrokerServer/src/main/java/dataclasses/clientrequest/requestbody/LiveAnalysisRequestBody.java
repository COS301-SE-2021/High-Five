package dataclasses.clientrequest.requestbody;

public class LiveAnalysisRequestBody implements RequestBody {
    private final String playLink;
    private final String publishLink;
    private final String streamId;

    public LiveAnalysisRequestBody(String playLink, String publishLink, String streamId) {
        this.playLink = playLink;
        this.publishLink = publishLink;
        this.streamId = streamId;
    }

    public String getPublishLink() {
        return publishLink;
    }

    public String getPlayLink() {
        return playLink;
    }

    public String getStreamId() {
        return streamId;
    }
}
