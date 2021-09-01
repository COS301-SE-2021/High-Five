package dataclasses.telemetry.collectors.decorators;

import dataclasses.telemetry.collectors.BaseCollector;
import dataclasses.telemetry.collectors.Collector;

public class CPUCollector extends BaseCollector {
    public CPUCollector(long weightMultiplier) {
        super(weightMultiplier);
    }

    @Override
    public long getUsage(String data) {
        long currentUsage = super.getUsage(data);

        //Do manipulation here;

        return currentUsage;
    }
}
