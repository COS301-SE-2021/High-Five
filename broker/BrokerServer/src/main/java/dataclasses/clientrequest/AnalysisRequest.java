package dataclasses.clientrequest;

public class AnalysisRequest {
    private final String analysisType;
    private final String authChallenge;

    public AnalysisRequest(String type, String challenge) {
        analysisType = type;
        authChallenge = challenge;
    }

    public String getAnalysisType() {
        return analysisType;
    }

    public String getAuthChallenge() {
        return authChallenge;
    }
}
