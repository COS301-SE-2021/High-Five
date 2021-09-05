package dataclasses.analysiscommand;

public class AnalysisCommand {
    private final String mediaType;
    private final String mediaId;

    public AnalysisCommand(String type, String id) {
        mediaType = type;
        mediaId = id;
    }

    public String toString() {
        return "{ \"analyze\": { \"mediatype\": \"" + mediaType + "\", \"mediaid\": " + mediaId + "\"}}";
    }
}
