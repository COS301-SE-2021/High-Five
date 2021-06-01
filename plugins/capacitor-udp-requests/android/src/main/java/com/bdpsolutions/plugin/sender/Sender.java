package com.bdpsolutions.plugin.sender;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.*;
import java.nio.charset.StandardCharsets;

/**
 *
 * @author Bieldt
 */
public class Sender{
    DatagramSocket udpSocket;
    byte[] payloadBytes;
    public Sender(String address, String port){
        try{
            udpSocket = new DatagramSocket();
            udpSocket.connect(InetAddress.getByName(address),Integer.parseInt(port));
        }catch(Exception e){
            e.printStackTrace();
        }
    }

    public void sendMessage(String payload){
        try {
            payloadBytes = stringToBytesASCII(payload);
            udpSocket.send(new DatagramPacket(payloadBytes,payloadBytes.length,InetAddress.getByName("192.168.10.1"),8889));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static byte[] stringToBytesASCII(String str) {
        byte[] b = new byte[str.length()];
        for (int i = 0; i < b.length; i++) {
            b[i] = (byte) str.charAt(i);
        }
        return b;
    }

}


