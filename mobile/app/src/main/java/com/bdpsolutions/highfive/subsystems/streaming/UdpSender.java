package com.bdpsolutions.highfive.subsystems.streaming;

import java.net.DatagramSocket;

public class UdpSender extends StreamSender{

    private DatagramSocket server;
    public UdpSender(){
        super();

    }

    @Override
    public void run() {

    }
}
