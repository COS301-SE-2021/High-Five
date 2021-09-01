package dataclasses.telemetry.builder;

import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.collectors.Collector;
import dataclasses.telemetry.collectors.GeneralCollector;

public class TelemetryBuilder {
    private Collector telemetryCollector;
    private String telemetryString;

    public TelemetryBuilder setPriority(TelemetryPriority priority) {
        switch (priority) {
            default -> telemetryCollector = new GeneralCollector();
        }
        return this;
    }

    public TelemetryBuilder setData(String data) {
        this.telemetryString = data;
        return this;
    }

    public Telemetry build() {
        return new TelemetryImpl(telemetryString, telemetryCollector);
    }

    private class TelemetryImpl extends Telemetry {

        public TelemetryImpl(String data, Collector collector) {
            super(data, collector);
        }

        @Override
        public long getTelemetry() {
            return telemetryCollector.getUsage(telemetryData);
        }
    }
}
