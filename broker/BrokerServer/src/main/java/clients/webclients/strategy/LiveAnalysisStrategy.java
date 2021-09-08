package clients.webclients.strategy;

import dataclasses.serverinfo.ServerInformation;
import dataclasses.clientrequest.AnalysisRequest;
import dataclasses.serverinfo.ServerInformationHolder;
import dataclasses.telemetry.builder.TelemetryBuilder;
import dataclasses.telemetry.builder.TelemetryCollector;

import java.io.BufferedWriter;
import java.io.IOException;

public class LiveAnalysisStrategy implements AnalysisStrategy{
    @Override
    public void processRequest(AnalysisRequest request, ServerInformationHolder information, BufferedWriter writer) throws IOException {

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
            infoString = info.toString();
        }

        writer.append(infoString).flush();

    }
}
