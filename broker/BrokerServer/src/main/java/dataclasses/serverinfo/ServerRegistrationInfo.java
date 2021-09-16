package dataclasses.serverinfo;

import com.google.gson.Gson;
import com.google.gson.annotations.SerializedName;

public class ServerRegistrationInfo {
    @SerializedName("serverId") String serverId;
    @SerializedName("timestamp") long timestamp;

    public ServerRegistrationInfo(String id, long timestamp) {
        serverId = id;
        this.timestamp = timestamp;
    }

    public String getServerId() {
        return serverId;
    }


    public long getTimestamp() {
        return timestamp;
    }


    public String toString() {
        return new Gson().toJson(this);
    }
}
