package dataclasses.clientrequest;


import dataclasses.clientrequest.requestbody.RequestBody;

public class AnalysisRequest {
    private final String authorization;
    private final String requestType;
    private final RequestBody body;
    private final String userId;

    public AnalysisRequest(String authorization, String requestType, String userId, RequestBody body) {
        this.authorization = authorization;
        this.requestType = requestType;
        this.userId = userId;
        this.body = body;
    }

    public String getAuthorization() {
        return authorization;
    }

    public String getRequestType() {
        return requestType;
    }

    public String getUserId() {
        return userId;
    }

    public RequestBody getBody() {
        return body;
    }
}
