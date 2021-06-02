package com.bdpsolutions.plugin.sender;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.*;
import java.nio.charset.StandardCharsets;

/**
 *
 * @author Ruan Bieldt
 */
public class Sender{
    DatagramSocket udpSocket;
    String address;
    int port;

    /**
     * Constructor for the sender class. Initialises the address and port and initialises the udp socket connection
     * @param address The IP address that a udp connection will be opened to
     * @param port The port that the udp connection will be opened to
     */
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

    /**
     * This function receives a payload string as a parameter, converts the string to a bytes array,
     * sends the payload bytes to the udp socket and then waits to receive a response. The response is then returned as a string.
     * @param payload The command the will be sent to the drone via udp datagram packet
     * @return The response received from the drone
     */
    public String sendMessage(String payload){
        try {
            byte[] payloadBytes = stringToBytesASCII(payload);
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

    /**
     * This function receives a payload string as a parameter, converts the string to a bytes array and sends the payload bytes to the udp socket
     * @param payload The string payload to send to the udp socket
     */
    public void sendMessageVoid(String payload){
        try {
            byte[] payloadBytes = stringToBytesASCII(payload);
            udpSocket.send(new DatagramPacket(payloadBytes,payloadBytes.length,InetAddress.getByName(address),port));

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    /**
     *
     * @param str The string to convert to a byte array
     * @return Returns a byte array of the string
     */
    public static byte[] stringToBytesASCII(String str) {
        byte[] b = new byte[str.length()];
        for (int i = 0; i < b.length; i++) {
            b[i] = (byte) str.charAt(i);
        }
        return b;
    }

    /**
     *Closes the udp socket connection
     */
    public void close(){
        udpSocket.close();
    }
}


