package clients.webclients.strategy;

import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.clientrequest.requestbody.LiveAnalysisRequestBody;
import dataclasses.serverinfo.ServerInformationHolder;
import logger.EventLogger;

import java.io.IOException;

public class LiveStreamStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, ConnectionHandler handler, String connectionId) throws IOException {
        LiveAnalysisRequestBody body = (LiveAnalysisRequestBody) request.getBody();
        EventLogger.getLogger().info(body.getPublishLinkDrone());
        String droneString = "{\"status\":\"success\",\"playLink\":\"" + body.getPublishLinkDrone() + "\",\"streamId\":\"" + body.getStreamId() +  "\"}";
        String infoString = "{\"status\":\"success\",\"streamId\":\"" + body.getStreamId() + "\",\"playLink\":\"none\"}";
        ResponseObject webResponse = new ResponseObject(request.getRequestType(), handler.getUserId(connectionId), infoString, connectionId);
        ResponseObject droneResponse = new ResponseObject("DroneResponse", handler.getUserId(connectionId), droneString, connectionId);
        handler.onNext(webResponse);
        handler.onNext(droneResponse);
    }
}
