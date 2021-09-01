package listeners.servers;

import dataclasses.websocket.Message;
import io.reactivex.rxjava3.core.Observer;
import listeners.ConnectionListener;
import org.w3c.dom.Document;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

public class KafkaMessageListener extends ConnectionListener {

    private final List<String> topics;

    public KafkaMessageListener(Observer<Message> notifier, List<String> topics) {
        super(notifier);
        this.topics = topics;
    }

    /**
     * Listens for new topics from the Kafka broker. When a new topic is found,
     * the notifier is called, with the name of the topic passed.
     */
    @Override
    public void listen() throws IOException, InterruptedException {

        while (true) {
            Runtime rt = Runtime.getRuntime();
            Process proc = rt.exec(System.getenv("KAFKA_LIST_TOPICS"));


            String line = null;

            BufferedReader inputStreamReader =
                    new BufferedReader(new InputStreamReader(proc.getInputStream()));
            while ((line = inputStreamReader.readLine()) != null) {
                if (!topics.contains(line)) {
                    Message msg = new Message();
                    msg.setContent(line);
                    notify(msg);
                }
            }

            BufferedReader errorStreamReader =
                    new BufferedReader(new InputStreamReader(proc.getErrorStream()));
            while ((line = errorStreamReader.readLine()) != null) {
                System.out.println(line);
            }
            Thread.sleep(1000L);
        }
    }
}
