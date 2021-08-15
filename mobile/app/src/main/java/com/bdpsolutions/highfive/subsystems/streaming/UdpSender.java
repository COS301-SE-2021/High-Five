package com.bdpsolutions.highfive.subsystems.streaming;

import java.net.DatagramSocket;

public class UdpSender implements StreamSender{
    private Thread runner;
    private boolean running;
    private DatagramSocket server;
    public UdpSender(){

    }

    @Override
    public void run() {

    }
}
