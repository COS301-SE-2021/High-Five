package clients.webclients.strategy;

import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.requestbody.LiveAnalysisRequestBody;
import dataclasses.serverinfo.ServerInformationHolder;

import java.io.IOException;

public class LiveStreamStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, ConnectionHandler handler, String connectionId) throws IOException {
        LiveAnalysisRequestBody body = (LiveAnalysisRequestBody) request.getBody();
        String infoString = "{\"status\":\"success\",\"streamId\":\"" + body.getStreamId() + "\"}";
        ResponseObject webResponse = new ResponseObject(request.getRequestType(), handler.getUserId(connectionId), infoString, null);
        handler.onNext(webResponse);
    }
}
