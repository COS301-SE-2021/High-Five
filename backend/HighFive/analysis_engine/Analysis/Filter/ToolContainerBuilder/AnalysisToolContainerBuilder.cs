using System;
using analysis_engine.Tools;
using analysis_engine.Util;

namespace analysis_engine.Filter.ToolContainerBuilder
{
    public class AnalysisToolContainerBuilder : ToolContainerBuilder
    {
        public override void buildContainer()
        {
            this._toolContainer = new AnalysisToolContainer();
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
            
        }

        public override ToolContainer getContainer(Pipe input)
        {
            return _toolContainer;
        }
    }
}