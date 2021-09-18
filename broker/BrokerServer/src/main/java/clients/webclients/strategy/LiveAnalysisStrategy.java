package clients.webclients.strategy;

import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.analysiscommand.commandbody.LiveAnalysisCommandBody;
import dataclasses.clientrequest.requestbody.LiveAnalysisRequestBody;
import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;
import logger.EventLogger;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.Producer;
import org.apache.kafka.clients.producer.ProducerRecord;
import org.apache.kafka.common.serialization.StringSerializer;

import java.io.IOException;
import java.util.Properties;

public class LiveAnalysisStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, ConnectionHandler handler, String connectionId) throws IOException {

        TelemetryBuilder builder = new TelemetryBuilder().setCollector(TelemetryCollector.ALL);
        ServerInformation info = information.get(builder);
        String userId = handler.getUserId(connectionId);


        String infoString;
        String droneString;
        if (info == null) {
            infoString = null;
            droneString = "{\"status\":\"error\",\"message\":\"No servers are available\"}";
        } else {
            Properties props = new Properties();
            props.put("bootstrap.servers", "localhost:9092");
            Producer<String, String> producer = new KafkaProducer<>(props, new StringSerializer(), new StringSerializer());

            //Create new command
            LiveAnalysisRequestBody body = (LiveAnalysisRequestBody) request.getBody();
            LiveAnalysisCommandBody commandBody = new LiveAnalysisCommandBody("http://192.168.11.153:5080/" + handler.getUserId(connectionId).replace("-","") + "/streams/" + body.getStreamId() + ".m3u8");
            AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getUserId(), commandBody);
            EventLogger.getLogger().info("Sending command to server " + info.getServerId());

            //Send command to server
            ProducerRecord<String, String> commandToSend = new ProducerRecord<>(info.getServerId(), 1, "Analyze", commandString.toString());
            producer.send(commandToSend);
            producer.close();
            droneString = "{\"status\":\"success\",\"playLink\":\"" + body.getPublishLinkDrone() + "\",\"streamId\":\"" + body.getStreamId() +  "\"}";
            infoString = "{\"status\":\"success\",\"streamId\":\"" + body.getStreamId() + "\",\"playLink\":\"none\"}";
        }



        ResponseObject droneResponse = new ResponseObject("DroneResponse", userId, droneString, connectionId);
        if (infoString != null) {
            ResponseObject webResponse = new ResponseObject(request.getRequestType(), userId, infoString, connectionId);
            handler.onNext(webResponse);
        }
        handler.onNext(droneResponse);
    }
}
