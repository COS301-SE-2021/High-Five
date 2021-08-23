package com.bdpsolutions.highfive.subsystems.streaming.udp;

import android.net.InetAddresses;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.bdpsolutions.highfive.subsystems.streaming.StreamSender;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;

public class UdpSender extends StreamSender {

    private DatagramSocket server;
    public UdpSender(){
        super();
    }

    @RequiresApi(api = Build.VERSION_CODES.N)
    public static DatagramChannel startClient() throws IOException {
        DatagramChannel client = DatagramChannelBuilder.bindChannel(null);
        return client;
    }

    public static void sendMessage(DatagramChannel client, String msg, SocketAddress serverAddress) throws IOException {
        ByteBuffer buffer = ByteBuffer.wrap(msg.getBytes());
        client.send(buffer, serverAddress);
    }
    @Override
    public void run() {

    }
}
