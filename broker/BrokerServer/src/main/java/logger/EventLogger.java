package logger;


import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.PrintWriter;
import java.io.StringWriter;

/**
 * Logger class to log events in the Broker system.
 */
public class EventLogger {
    private static EventLogger _instance;
    private final Logger logger = LogManager.getLogger(EventLogger.class.getName());

    private EventLogger() {}

    public static EventLogger getLogger() {
        if (_instance == null) {
            _instance = new EventLogger();
        }
        return _instance;
    }

    public void info(String message) {
        String callerClass = new Exception().getStackTrace()[1].getClassName();
        logger.info(callerClass + ": " + message);
    }

    public void error(String message) {
        String callerClass = new Exception().getStackTrace()[1].getClassName();
        logger.error(callerClass + ": " + message);
    }

    public void warn(String message) {
        String callerClass = new Exception().getStackTrace()[1].getClassName();
        logger.warn(callerClass + ": " + message);
    }

    public void debug(String message) {
        String callerClass = new Exception().getStackTrace()[1].getClassName();
        logger.debug(callerClass + ": " + message);
    }

    public void logException(Throwable e) {
        StringWriter sw = new StringWriter();
        PrintWriter pw = new PrintWriter(sw);
        e.printStackTrace(pw);
        EventLogger.getLogger().error(sw.toString());
    }
}
