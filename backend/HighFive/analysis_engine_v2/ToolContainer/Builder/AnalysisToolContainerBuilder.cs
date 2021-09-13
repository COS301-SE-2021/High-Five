using System;

namespace analysis_engine
{
    public class AnalysisToolContainerBuilder : ToolContainerBuilder
    {
        public override void buildContainer(bool last)
        {
            this._toolContainer = new AnalysisToolContainer();
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

        public override void addTool(String toolName)
        {
            var toolBuilder = new AnalysisToolBuilder();
            toolBuilder.BuildTool(toolName);
            _toolContainer.Tool = toolBuilder.GetTool();
        }

        public override ToolContainer getContainer()
        {
            return _toolContainer;
        }
    }
}