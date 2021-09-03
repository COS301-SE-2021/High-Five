import org.apache.catalina.startup.Tomcat;
import servicelocator.ServiceLocator;

import java.io.*;

/**
 * Entrypoint for the broker system. Runs the broker class and starts an embedded
 * Tomcat server to accept WebSocket connections.
 */
public class Main {
    public static void main(String[] args) throws Exception {
        ServiceLocator locator = ServiceLocator.getInstance();
        Broker b = new Broker();


        Tomcat tomcat = new Tomcat();
        String port = "8080"; // Also change in index.html
        tomcat.setPort(Integer.parseInt(port));
        tomcat.addWebapp("/", new File("tempwebapp").getAbsolutePath());
        tomcat.start();
        tomcat.getServer().await();

        while (true) {
            Thread.sleep(1000L);
        }
    }
}
