package com.bdpsolutions.highfive.subsystems.streaming.udp;

import android.os.Build;

import androidx.annotation.RequiresApi;

import java.io.IOException;
import java.net.SocketAddress;
import java.nio.channels.DatagramChannel;

public class DatagramChannelBuilder {

    public static DatagramChannel openChannel() throws IOException {
        DatagramChannel datagramChannel = DatagramChannel.open();
        return datagramChannel;
    }

    @RequiresApi(api = Build.VERSION_CODES.N)
    public static DatagramChannel bindChannel(SocketAddress local) throws IOException {
        return openChannel().bind(local);
    }
}
