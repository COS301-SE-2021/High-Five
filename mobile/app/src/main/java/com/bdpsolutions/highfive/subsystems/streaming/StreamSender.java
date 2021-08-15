package com.bdpsolutions.highfive.subsystems.streaming;

import java.util.concurrent.ConcurrentLinkedQueue;

public abstract class StreamSender implements Runnable{
    private Thread runner;
    protected static boolean running,streaming;
    protected static ConcurrentLinkedQueue<byte[]> inputBuffer = new ConcurrentLinkedQueue<>();

    public StreamSender(){
        runner = new Thread(this);
        running = true;
        runner.start();
    }

    public static void startStream(){
        streaming = true;
    }
    public static void stopStream(){
        streaming = false;
    }

    protected static void pushToBuffer(byte[] data){
        inputBuffer.add(data);
    }



}
