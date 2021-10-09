import logger.EventLogger;
import managers.concurrencymanager.ConcurrencyManager;
import managers.requestqueue.RequestQueue;
import org.apache.catalina.startup.Tomcat;

import java.io.*;

/**
 * Entrypoint for the broker system. Runs the broker class and starts an embedded
 * Tomcat server to accept WebSocket connections.
 */
public class Main {

    public static void main(String[] args) throws Exception {
        EventLogger.getLogger().info("Starting Broker");
        ConcurrencyManager.getInstance(); //initialise the concurrency manager
        RequestQueue.getInstance();
        Broker b = new Broker();

        EventLogger.getLogger().info("Starting Embedded Tomcat server");
        Tomcat tomcat = new Tomcat();
        String port = "8080";
        tomcat.setHostname("0.0.0.0");
        tomcat.setPort(Integer.parseInt(port));
        tomcat.addWebapp("/", new File("tempwebapp").getAbsolutePath());
        tomcat.start();
        tomcat.getServer().await();

        while (true) {
            Thread.sleep(1000L);
        }
    }
}
