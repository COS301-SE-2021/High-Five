package logger;


import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

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
        logger.info(message);
    }

    public void error(String message) {
        logger.error(message);
    }
}
