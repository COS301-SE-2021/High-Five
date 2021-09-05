package dataclasses.clientrequest;


public class AnalysisRequest {
    private final String analysisType;
    private final String authChallenge;
    private final String mediaType;
    private final String mediaID;

    public AnalysisRequest(String type, String challenge, String mediaType, String mediaID) {
        analysisType = type;
        authChallenge = challenge;
        this.mediaType = mediaType;
        this.mediaID = mediaID;
    }

    public String getAnalysisType() {
        return analysisType;
    }

    public String getAuthChallenge() {
        return authChallenge;
    }

    public String getMediaType() {
        return mediaType;
    }

    public String getMediaID() {
        return mediaID;
    }
}
