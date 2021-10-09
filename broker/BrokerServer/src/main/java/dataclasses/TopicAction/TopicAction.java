package dataclasses.TopicAction;

public class TopicAction {
    public enum Action {
        ADD_TOPIC,
        DELETE_TOPIC
    }

    public TopicAction(Action action, String message) {
        this.action = action;
        this.message = message;
    }

    public Action action;
    public String message;
}
