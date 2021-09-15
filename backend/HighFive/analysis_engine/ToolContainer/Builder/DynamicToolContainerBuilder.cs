using System;
using analysis_engine.BrokerClient;

namespace analysis_engine
{
    public class DynamicToolContainerBuilder : ToolContainerBuilder
    {
        public override void buildContainer(bool last)
        {
            this._toolContainer = new DynamicToolContainer();
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
            var toolBuilder = new DynamicToolBuilder();
            toolBuilder.BuildTool(toolName);
            _toolContainer.Tool = toolBuilder.GetTool();
        }

        public override ToolContainer getContainer()
        {
            return _toolContainer;
        }
    }
}