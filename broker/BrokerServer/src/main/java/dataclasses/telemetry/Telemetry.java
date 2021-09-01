package dataclasses.telemetry;

import dataclasses.telemetry.collectors.Collector;

public abstract class Telemetry {
    protected String telemetryData;
    protected final Collector telemetryCollector;

    public Telemetry(String data, Collector collector) {
        this.telemetryData = data;
        this.telemetryCollector = collector;
    }

    public abstract long getTelemetry();
}
