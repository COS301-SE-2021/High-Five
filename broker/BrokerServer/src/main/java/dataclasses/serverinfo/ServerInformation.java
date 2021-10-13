package dataclasses.serverinfo;

public class ServerInformation {

    private long usage;
    private final String serverId;
    private volatile boolean isBusy = false;

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

    public boolean isBusy() {
        return isBusy;
    }

    public void setBusy(boolean busy) {
        isBusy = busy;
    }
}
