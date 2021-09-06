package dataclasses.telemetry.collectors;

import dataclasses.serverinfo.ServerUsage;

public interface Collector {
    short getUsage(ServerUsage data);
    void setWeight(short weight);
    void setNextCollector(Collector next);
}
