package dataclasses.serverinfo;

public class ServerInformation {

    private String address;
    private String serverId;
    private String port;
    private String credentials;
    private long usage;

    public ServerInformation(String address, String serverId, String port, String credentials) {
        this.address = address;
        this.serverId = serverId;
        this.port = port;
        this.credentials = credentials;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getPort() {
        return port;
    }

    public void setPort(String port) {
        this.port = port;
    }

    public String getCredentials() {
        return credentials;
    }

    public void setCredentials(String credentials) {
        this.credentials = credentials;
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

    public void setServerId(String serverId) {
        this.serverId = serverId;
    }

    public String toString() {
        return "{" +
                "\"id\": \"" + serverId + "\"," +
                "\"address\": \"" + address + "\"," +
                "\"port\": \"" + port + "\"," +
                "\"credentials\": \"" + credentials + "\"}";
    }

    public ServerInformation clone() {
        return new ServerInformation(address, serverId, port, credentials);
    }
}
