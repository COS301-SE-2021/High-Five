package dataclasses.sockets.codecs;

import com.google.gson.Gson;
import dataclasses.sockets.ClientRequest;

import javax.websocket.*;

public class RequestDecoder implements Decoder.Text<ClientRequest> {

    private static Gson gson = new Gson();

    @Override
    public ClientRequest decode(String s) throws DecodeException {
        return gson.fromJson(s, ClientRequest.class);
    }

    @Override
    public boolean willDecode(String s) {
        return (s != null);
    }

    @Override
    public void init(EndpointConfig endpointConfig) {
        // Custom initialization logic
    }

    @Override
    public void destroy() {
        // Close resources
    }
}