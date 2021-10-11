package dataclasses.clientrequest.requestbody;

public class LiveAnalysisRequestBody implements RequestBody {
    private final String playLinkWeb;
    private final String publishLinkDrone;
    private final String playLinkAnalysis;
    private final String publishLinkAnalysis;
    private final String streamId;

    public LiveAnalysisRequestBody(String playLinkWeb, String publishLinkDrone, String playLinkAnalysis, String publishLinkAnalysis, String streamId) {
        this.playLinkWeb = playLinkWeb;
        this.publishLinkDrone = publishLinkDrone;
        this.publishLinkAnalysis = publishLinkAnalysis;
        this.playLinkAnalysis = playLinkAnalysis;
        this.streamId = streamId;
    }

    public String getStreamId() {
        return streamId;
    }

    public String getPlayLinkWeb() {
        return playLinkWeb;
    }

    public String getPublishLinkDrone() {
        return publishLinkDrone;
    }

    public String getPlayLinkAnalysis() {
        return playLinkAnalysis;
    }

    public String getPublishLinkAnalysis() {
        return publishLinkAnalysis;
    }
}
