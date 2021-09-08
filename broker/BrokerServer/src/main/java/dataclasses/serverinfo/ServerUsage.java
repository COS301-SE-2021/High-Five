package dataclasses.serverinfo;

public class ServerUsage {
    public short cpu;
    public short gpu;
    public short net;
    public short disk;

    public ServerUsage clone() {
        ServerUsage clone = new ServerUsage();
        clone.cpu = cpu;
        clone.gpu = gpu;
        clone.net = net;
        clone.disk = disk;

        return clone;
    }
}
