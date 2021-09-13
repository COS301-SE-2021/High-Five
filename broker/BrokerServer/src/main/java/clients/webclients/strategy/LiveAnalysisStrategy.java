package clients.webclients.strategy;

import clients.webclients.connectionhandler.ConnectionHandler;
import clients.webclients.connectionhandler.ResponseObject;
import dataclasses.analysiscommand.AnalysisCommand;
import dataclasses.analysiscommand.commandbody.LiveAnalysisCommandBody;
import dataclasses.analysiscommand.commandbody.StoredMediaCommandBody;
import dataclasses.clientrequest.requestbody.LiveAnalysisRequestBody;
import dataclasses.clientrequest.requestbody.StoredMediaRequestBody;
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

import java.io.BufferedWriter;
import java.io.IOException;
import java.util.Properties;

public class LiveAnalysisStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, ConnectionHandler handler, String userId) throws IOException {

        /*
        Live streaming requires CPU usage, but it prioritises GPU usage
        and needs good network connection.
         */
        TelemetryBuilder usageTelemetry = new TelemetryBuilder()
                .setCollector(TelemetryCollector.CPU)
                .setCollector(TelemetryCollector.GPU_PRIORITY)
                .setCollector(TelemetryCollector.NETWORK_PRIORITY);

        ServerInformation info = information.get(usageTelemetry);


        String infoString;
        if (info == null) {
            infoString = "No servers are available";
        } else {
            Properties props = new Properties();
            props.put("bootstrap.servers", "localhost:9092");
            Producer<String, String> producer = new KafkaProducer<>(props, new StringSerializer(), new StringSerializer());

            //Create new command
            LiveAnalysisRequestBody body = (LiveAnalysisRequestBody) request.getBody();
            LiveAnalysisCommandBody commandBody = new LiveAnalysisCommandBody(body.getStreamId(), body.getPublishLink());
            AnalysisCommand commandString = new AnalysisCommand(request.getRequestType(), request.getUserId(), commandBody);
            EventLogger.getLogger().info("Sending command to server " + info.getServerId());

            //Send command to server
            ProducerRecord<String, String> commandToSend = new ProducerRecord<>(info.getServerId(), 1, "Analyze", commandString.toString());
            producer.send(commandToSend);
            producer.close();
            infoString = "{\"playLink\":\"" + body.getPlayLink() + "\",\"serverInformation\":" + info + "}";
        }



        ResponseObject responseObject = new ResponseObject(request.getRequestType(), userId, infoString, null);
        handler.onNext(responseObject);
    }
}
