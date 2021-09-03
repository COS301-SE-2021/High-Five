using analysis_engine.Analysis.Tools.Builders;
using analysis_engine.Util;

namespace analysis_engine.Filter.ToolContainerBuilder
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