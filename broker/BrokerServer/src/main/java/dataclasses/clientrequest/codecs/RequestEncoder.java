package dataclasses.clientrequest.codecs;

import com.google.gson.Gson;
import dataclasses.clientrequest.AnalysisRequest;

import javax.websocket.EncodeException;
import javax.websocket.Encoder;
import javax.websocket.EndpointConfig;

public class RequestEncoder implements Encoder.Text<AnalysisRequest> {

    private static final Gson gson = new Gson();

    @Override
    public String encode(AnalysisRequest message) throws EncodeException {
        return gson.toJson(message);
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
