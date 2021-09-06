package dataclasses.telemetry.collectors;

import dataclasses.serverinfo.ServerUsage;

public class BaseCollector implements Collector {

    private Collector nextCollector;
    protected short weight = 0;
    protected short weightMultiplier = 0;

    public BaseCollector(short weightMultiplier) {
        this.weightMultiplier = weightMultiplier;
    }

    @Override
    public short getUsage(ServerUsage data) {
        if (nextCollector != null) {
            return nextCollector.getUsage(data);
        } else {
            return 0;
        }
    }

    @Override
    public void setNextCollector(Collector nextCollector) {
        if (this.nextCollector != null) {
            this.nextCollector.setNextCollector(nextCollector);
        } else {
            this.nextCollector = nextCollector;
        }
    }

    @Override
    public void setWeight(short weight) {
        this.weight = weight;
        if (nextCollector != null) {
            nextCollector.setWeight(weight);
        }
    }
}
