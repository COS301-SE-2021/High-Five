package clients.webclients.connectionhandler;

public class ResponseObject {
    public String requestType;
    public String userId;
    public String data;
    public String connectionId;

    public ResponseObject(String requestType, String userId, String data, String connectionId) {
        this.data = data;
        this.userId = userId;
        this.requestType = requestType;
        this.connectionId = connectionId;
    }

}
