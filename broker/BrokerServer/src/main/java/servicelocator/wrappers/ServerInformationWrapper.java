package servicelocator.wrappers;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonElement;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.serverinfo.codecs.ServerInformationDecoder;
import servicelocator.ServiceLocator;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Type;

public class ServerInformationWrapper {
    public static ServerInformationDecoder get() throws InvocationTargetException, InstantiationException, IllegalAccessException {
        return ServiceLocator
                .getInstance()
                .<ServerInformationDecoder>createClass("ServerInfoDecoder")
                .newInstance();
    }
}
