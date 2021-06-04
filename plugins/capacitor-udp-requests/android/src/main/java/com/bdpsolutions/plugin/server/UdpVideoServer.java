package com.bdpsolutions.plugin.server;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.SocketException;

/**
 *
 */
public class UdpVideoServer implements Runnable{
    private boolean running;
    Thread serverRunner;
    DatagramSocket socket;
    int serverPort;

    /**
     *
     */
    public UdpVideoServer(){
        serverPort = 11111;
        running = true;
        serverRunner = new Thread(this);
        serverRunner.start();
    }

    /**
     *
     */
    public void stopServer(){
        running = false;
    }

    /**
     *
     */
    @Override
    public void run() {
        try{
            System.out.println("Starting server");
            socket = new DatagramSocket(serverPort);
            while(running){
                byte[] buffer = new byte[1460];
                DatagramPacket packet = new DatagramPacket(buffer,buffer.length);
                System.out.println("Waiting for packet");
                socket.receive(packet);
                System.out.println("Packet receieved length: "+ packet.getLength());
            }
        }catch(IOException ioe){
            System.out.println(ioe.getMessage());
        }
    }
}
