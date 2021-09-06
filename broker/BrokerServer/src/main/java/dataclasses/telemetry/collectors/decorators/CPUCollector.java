package dataclasses.telemetry.collectors.decorators;

import dataclasses.serverinfo.ServerUsage;
import dataclasses.telemetry.collectors.BaseCollector;
import dataclasses.telemetry.collectors.Collector;

public class CPUCollector extends BaseCollector {
    public CPUCollector(short weightMultiplier) {
        super(weightMultiplier);
    }

    @Override
    public short getUsage(ServerUsage data) {
        short currentUsage = super.getUsage(data);

        if (weightMultiplier > 1) {
            currentUsage = (short) (currentUsage * (1 - (weight / (weightMultiplier-1))));
        }

        short usage = (short) (data.cpu / weight);

        if (weightMultiplier > 1) {
            usage = (short) (usage * (1 - (weight / (weightMultiplier-1))));
        }


        return (short) (currentUsage + usage);
    }
}
