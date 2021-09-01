package dataclasses.telemetry.builder;

import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.collectors.BaseCollector;
import dataclasses.telemetry.collectors.Collector;
import dataclasses.telemetry.collectors.decorators.CPUCollector;
import dataclasses.telemetry.collectors.decorators.GPUCollector;

public class TelemetryBuilder {
    private Collector telemetryCollector;
    private String telemetryString;
    private long weight = 1;

    public TelemetryBuilder setCollector(TelemetryCollector priority) {
        if (telemetryCollector == null) {
            telemetryCollector = new BaseCollector(0);
        }
        Collector newCollector = null;
        switch (priority) {
            case CPU -> newCollector = new CPUCollector(1);
            case CPU_PRIORITY -> newCollector = new CPUCollector(2);
            case GPU -> newCollector = new GPUCollector(1);
            case GPU_PRIORITY -> newCollector = new GPUCollector(2);
            default -> {
                setCollector(TelemetryCollector.CPU);
                setCollector(TelemetryCollector.GPU);
            }
        }
        telemetryCollector.setNextCollector(newCollector);
        telemetryCollector.setWeight(weight++);
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
