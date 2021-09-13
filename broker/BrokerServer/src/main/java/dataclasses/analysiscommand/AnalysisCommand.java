package dataclasses.analysiscommand;

import dataclasses.analysiscommand.commandbody.CommandBody;

import java.util.UUID;

public class AnalysisCommand {
    private final String commandType;
    private final CommandBody body;
    private final String userId;

    public AnalysisCommand(String type, String userId, CommandBody body) {
        commandType = type;
        this.userId = userId;
        this.body = body;
    }

    public String toString() {
        String analysisId = UUID.randomUUID().toString();
        return "{\"CommandId\":\"" + analysisId + "\",\"CommandType\":\"" +
                commandType + "\",\"UserId\":\"" + userId + "\",\"Body\":"+ body.toString() + "}";
    }
}
