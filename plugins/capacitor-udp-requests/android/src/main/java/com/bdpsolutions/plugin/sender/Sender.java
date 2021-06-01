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
    String address;
    int port;
    byte[] payloadBytes;
    public Sender(String address, String port){
        try{
            this.address = address;
            this.port = Integer.parseInt(port);
            udpSocket = new DatagramSocket();
            udpSocket.connect(InetAddress.getByName(address),Integer.parseInt(port));
        }catch(Exception e){
            e.printStackTrace();
        }
    }

    public String sendMessage(String payload){
        try {
            payloadBytes = stringToBytesASCII(payload);
            udpSocket.send(new DatagramPacket(payloadBytes,payloadBytes.length,InetAddress.getByName(address),port));
            byte[] receivePayload = new byte[64];
            DatagramPacket response = new DatagramPacket(receivePayload, receivePayload.length);
            udpSocket.receive(response);
            return new String(response.getData(),0,response.getLength(), StandardCharsets.US_ASCII);
        } catch (IOException e) {
            e.printStackTrace();
            return "error";
        }
    }

    public static byte[] stringToBytesASCII(String str) {
        byte[] b = new byte[str.length()];
        for (int i = 0; i < b.length; i++) {
            b[i] = (byte) str.charAt(i);
        }
        return b;
    }

    public void close(){
        udpSocket.close();
    }
}


