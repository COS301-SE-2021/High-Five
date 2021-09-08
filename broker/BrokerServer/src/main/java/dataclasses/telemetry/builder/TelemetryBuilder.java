package dataclasses.telemetry.builder;

import dataclasses.serverinfo.ServerUsage;
import dataclasses.telemetry.Telemetry;
import dataclasses.telemetry.collectors.BaseCollector;
import dataclasses.telemetry.collectors.Collector;
import dataclasses.telemetry.collectors.decorators.CPUCollector;
import dataclasses.telemetry.collectors.decorators.GPUCollector;

public class TelemetryBuilder {
    private Collector telemetryCollector;
    private ServerUsage telemetryUsage;
    private short weight = 1;

    public TelemetryBuilder setCollector(TelemetryCollector priority) {
        if (telemetryCollector == null) {
            telemetryCollector = new BaseCollector((short) 0);
        }
        Collector newCollector = null;
        switch (priority) {
            case CPU -> newCollector = new CPUCollector((short) 1);
            case CPU_PRIORITY -> newCollector = new CPUCollector((short) 2);
            case GPU -> newCollector = new GPUCollector((short) 1);
            case GPU_PRIORITY -> newCollector = new GPUCollector((short) 2);
            default -> {
                setCollector(TelemetryCollector.CPU);
                setCollector(TelemetryCollector.GPU);
            }
        }
        if (newCollector != null) {
            telemetryCollector.setNextCollector(newCollector);
            telemetryCollector.setWeight(weight++);
        }
        return this;
    }

    public TelemetryBuilder setData(ServerUsage data) {
        this.telemetryUsage = data;
        return this;
    }

    public Telemetry build() {
        return new TelemetryImpl(telemetryUsage, telemetryCollector);
    }

    private class TelemetryImpl extends Telemetry {

        public TelemetryImpl(ServerUsage data, Collector collector) {
            super(data, collector);
        }

        @Override
        public long getTelemetry() {
            return telemetryCollector.getUsage(telemetryData);
        }
    }
}
