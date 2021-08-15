package com.bdpsolutions.highfive.subsystems.streaming.udp;

import java.io.IOException;
import java.nio.channels.DatagramChannel;

public class DatagramChannelBuilder {
    public static DatagramChannel openChannel() throws IOException {
        DatagramChannel datagramChannel = DatagramChannel.open();
        return datagramChannel;
    }
}
