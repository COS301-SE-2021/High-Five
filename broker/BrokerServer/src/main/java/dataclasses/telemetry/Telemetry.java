package dataclasses.telemetry;

import dataclasses.serverinfo.ServerUsage;
import dataclasses.telemetry.collectors.Collector;

public abstract class Telemetry {
    protected ServerUsage telemetryData;
    protected final Collector telemetryCollector;

    public Telemetry(ServerUsage data, Collector collector) {
        this.telemetryData = data;
        this.telemetryCollector = collector;
    }

    public abstract long getTelemetry();
}
