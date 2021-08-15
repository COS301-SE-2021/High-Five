package com.bdpsolutions.highfive.subsystems.streaming.udp;

import android.net.InetAddresses;

import com.bdpsolutions.highfive.subsystems.streaming.StreamSender;

import java.io.IOException;
import java.net.DatagramSocket;

public class UdpSender extends StreamSender {

    private DatagramSocket server;
    public UdpSender(InetAddresses address, int port){
        super();
        try{

        }catch(IOException ioe){

        }

    }

    @Override
    public void run() {

    }
}
