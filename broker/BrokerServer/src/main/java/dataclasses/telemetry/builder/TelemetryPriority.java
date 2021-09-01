package dataclasses.telemetry.builder;

public enum TelemetryPriority {
    ALL,
    CPU_PRIORITY,
    CPU_ONLY,
    GPU_PRIORITY,
    GPU_ONLY,
    DISK_PRIORITY,
    DISK_ONLY,
    NETWORK_PRIORITY,
    NETWORK_ONLY
}
