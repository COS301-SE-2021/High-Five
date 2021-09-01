package clients.servers;

import dataclasses.websocket.Message;
import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.Observer;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;

public class KafkaServerParticipant extends ServerParticipant {

    private final ArrayList<String> topics;

    public KafkaServerParticipant(Observer<Message> observable, ArrayList<String> topics) {
        super(observable);
        this.topics = topics;
    }

    @Override
    public boolean heartbeat() {
        return beat;
    }

    @Override
    public void listen() throws InterruptedException {
        while (true) {
            for (String topic: topics) {
                Runtime rt = Runtime.getRuntime();
                Process proc = null;
                String exec = System.getenv("KAFKA_GET_MESSAGES").replace("{topic}", topic);
                try {
                    proc = rt.exec(exec);
                } catch (IOException e) {
                    beat = false;
                    return;
                }

                String line = null;

                BufferedReader inputStreamReader =
                        new BufferedReader(new InputStreamReader(proc.getInputStream()));
                while (true) {
                    try {
                        if ((line = inputStreamReader.readLine()) == null) break;
                    } catch (IOException e) {
                        beat = false;
                        return;
                    }
                    Message msg = new Message();
                    msg.setContent(line);
                    notify(msg);
                    topics.add(line);
                }
            }
            Thread.sleep(1000L);
        }
    }
}
