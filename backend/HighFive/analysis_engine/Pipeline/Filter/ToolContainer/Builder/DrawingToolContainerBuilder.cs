namespace analysis_engine
{
    public class DrawingToolContainerBuilder : ToolContainerBuilder
    {
        public override void buildContainer()
        {
            this._toolContainer = new DrawingToolContainer();
        }

        public override void addInput(Pipe input)
        {
            _toolContainer.Input = input;
        }

        public override void addOutput(Pipe output)
        {
            _toolContainer.Output = output;
        }

        public override void addTool(string toolName)
        {
            var toolBuilder = new DrawingToolBuilder();
            toolBuilder.BuildTool(toolName);
            _toolContainer.Tool = toolBuilder.GetTool();
        }

        public override ToolContainer getContainer()
        {
            return _toolContainer;
        }
    }
}