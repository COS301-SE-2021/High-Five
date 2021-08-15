package com.bdpsolutions.highfive.subsystems.streaming;

import java.util.concurrent.ConcurrentLinkedQueue;

public interface StreamSender extends Runnable{

    static ConcurrentLinkedQueue<byte[]> inputBuffer = new ConcurrentLinkedQueue<>();

    static void pushToBuffer(byte[] data){
        inputBuffer.add(data);
    }

}
