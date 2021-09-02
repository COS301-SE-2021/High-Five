import org.apache.catalina.startup.Tomcat;

import java.io.*;

public class Main {
    public static void main(String[] args) throws Exception {
        Broker b = new Broker();
        System.out.println("HELLO WORLD");

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
