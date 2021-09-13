package dataclasses.serverinfo;

public class ServerRegistrationInfo {
    private String serverId;
    private long timestamp;

    public ServerRegistrationInfo(String id, long timestamp) {
        serverId = id;
        this.timestamp = timestamp;
    }

    public String getServerId() {
        return serverId;
    }

    public void setServerId(String serverId) {
        this.serverId = serverId;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }

    public String toString() {
        return "{\"serverId\":\"" + serverId + "\",\"timestamp\":" + timestamp + "}";
    }
}
