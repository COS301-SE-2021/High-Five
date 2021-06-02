package com.bdpsolutions.plugin.sender;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.*;
/**
 *
 * @author Bieldt
 */
public class ESPConnection implements Runnable{
    DatagramSocket espSock;
    Thread runner = null;
    Boolean running = false;

    ByteBuffer buf;
    byte[] bufferedChannels;
    ESPConnection(){
        try{
            espSock = new DatagramSocket();
            espSock.connect(InetAddress.getByName("192.168.10.1"),8889);
            runner = new Thread(this);
            runner.start();
            running = true;
            buf = ByteBuffer.allocate(10);
        }catch(Exception e){
            e.printStackTrace();
        }
    }

    @Override
    public void run() {
        while(running){
            try{
                Thread.sleep(1);

                espSock.send(new DatagramPacket(bufferedChannels,10,InetAddress.getByName("192.168.10.1"),8889));
                //send the array of bytes to the ip address entered as a datagrampacket
            }catch(Exception e){
                e.printStackTrace();
            }
        }
    }
}

//public class Sender {
//    private DatagramSocket socket;
//    private InetAddress address;
//    private int port;
//    private byte[] buffer;
//
//    public Sender(String address, String port) {
//        try {
//            this.socket = new DatagramSocket();
//            this.address = InetAddress.getByName(address);
//            this.port = Integer.parseInt(port);
//        } catch (Exception e) {
//            System.out.println(e.toString());
//            this.socket.close();
//        }
//    }
//
//
//    public String sendMessage(String message){
//        buffer= message.getBytes();
//        DatagramPacket packet = new DatagramPacket(buffer, buffer.length, this.address,this.port);
//        try{
//            socket.send(packet);
//            packet= new DatagramPacket(buffer, buffer.length);
//            socket.receive(packet);
//            String received = new String(packet.getData(), 0, packet.getLength());
//            return received;
//        }catch (Exception e){
//
//        }
//        return "Error";
//    }
//
//    public void close(){
//        this.socket.close();
//    }
//}
