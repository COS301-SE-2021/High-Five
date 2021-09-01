package dataclasses.telemetry.collectors;

public class BaseCollector implements Collector {

    private Collector nextCollector;
    private long weight = 0;
    private long weightMultiplier = 0;
    private static long MAX_USAGE = 100;

    public BaseCollector(long weightMultiplier) {
        this.weightMultiplier = weightMultiplier;
    }

    @Override
    public long getUsage(String data) {
        if (nextCollector != null) {
            return nextCollector.getUsage(data);
        } else {
            return MAX_USAGE;
        }
    }

    @Override
    public void setNextCollector(Collector nextCollector) {
        if (this.nextCollector != null) {
            nextCollector.setNextCollector(nextCollector);
        } else {
            this.nextCollector = nextCollector;
        }
    }

    @Override
    public void setWeight(long weight) {
        this.weight = weight;
        if (nextCollector != null) {
            nextCollector.setWeight(weight);
        }
    }
}
