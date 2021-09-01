package dataclasses.telemetry.collectors;

public interface Collector {
    long getUsage(String data);
    void setWeight(long weight);
    void setNextCollector(Collector next);
}
