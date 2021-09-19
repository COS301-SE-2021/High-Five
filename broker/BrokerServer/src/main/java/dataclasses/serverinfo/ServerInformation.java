package dataclasses.serverinfo;

public class ServerInformation {

    private long usage;
    private final String serverId;

    public ServerInformation(String serverId) {
        this.serverId = serverId;
    }

    public long getUsage() {
        return usage;
    }

    public void setUsage(long usage) {
        this.usage = usage;
    }

    public String getServerId() {
        return serverId;
    }


    public ServerInformation clone() {
        ServerInformation info = new ServerInformation(serverId);
        info.setUsage(usage);
        return info;
    }
}
