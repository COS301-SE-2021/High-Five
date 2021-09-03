package servicelocator;

import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import org.apache.commons.io.IOUtil;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.StringWriter;
import java.lang.reflect.Constructor;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.locks.ReentrantLock;

public class ServiceLocator {

    private static ServiceLocator _instance = null;
    private static final ReentrantLock lock = new ReentrantLock();
    private final Map<String, String> services = new HashMap<>();
    private static final boolean ISJAR = false;

    private ServiceLocator() throws IOException {

        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        InputStream is = classloader.getResourceAsStream("services.json");

        if (is == null) {
            System.err.println("Failed to load services.json");
            System.exit(-1);
        }

        StringWriter writer = new StringWriter();
        IOUtil.copy(is, writer, "UTF-8");

        JsonArray serviceInfo = new Gson().fromJson(writer.toString(), JsonArray.class);

        for (JsonElement item : serviceInfo) {
            services.put(item.getAsJsonObject().get("name").getAsString(), item.getAsJsonObject().get("path").getAsString());
        }
    }

    @SuppressWarnings("unchecked")
    public <T> Constructor<T> createClass(String name, Class<?>... paramTypes){
        try {
            return (Constructor<T>) Class.forName(services.get(name)).getDeclaredConstructor(paramTypes);
        } catch (NoSuchMethodException | ClassNotFoundException e) {
            e.printStackTrace();
            System.exit(-1);
        }
        return null;
    }

    public static ServiceLocator getInstance() {
        lock.lock();
        try {
            if (_instance == null) {
                _instance = new ServiceLocator();
            }
            return _instance;
        } catch (IOException e) {
            e.printStackTrace();
            System.exit(-1);
        } finally {
            lock.unlock();
        }
        return null;
    }

}
