package com.bdpsolutions.plugin.sender;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.*;
/**
 *
 * @author Bieldt
 */
public class Sender{
    DatagramSocket udpSocket;
    byte[] payloadBytes;
    public Sender(String address, String port, int payloadLength){
        try{
            udpSocket = new DatagramSocket();
            udpSocket.connect(InetAddress.getByName(address),Integer.valueOf(port));
            payloadBytes = new byte[payloadLength];
        }catch(Exception e){
            e.printStackTrace();
        }
    }

    public void sendMessage(String payload){
        try {
//            payloadBytes = payload.getBytes("")
            udpSocket.send(new DatagramPacket(payloadBytes,payloadBytes.length,InetAddress.getByName("192.168.10.1"),8889));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }


}


