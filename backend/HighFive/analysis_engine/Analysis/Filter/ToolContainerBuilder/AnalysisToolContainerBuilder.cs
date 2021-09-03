using System;
using analysis_engine.Analysis.Tools.Builders;
using analysis_engine.Filter;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Filter.ToolContainerBuilder
{
    public class AnalysisToolContainerBuilder : analysis_engine.Filter.ToolContainerBuilder.ToolContainerBuilder
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