namespace analysis_engine
{
    public class DroneToolContainerBuilder : ToolContainerBuilder
    {
        public override void buildContainer(bool last)
        {
            this._toolContainer = new DroneToolContainer();
            _toolContainer.Last = last;
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
            var toolBuilder = new DroneToolBuilder();
            toolBuilder.BuildTool(toolName);
            _toolContainer.Tool = toolBuilder.GetTool();
        }

        public override ToolContainer getContainer()
        {
            return _toolContainer;
        }
    }
}