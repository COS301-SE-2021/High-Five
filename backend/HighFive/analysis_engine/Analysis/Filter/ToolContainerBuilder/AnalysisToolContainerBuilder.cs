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
        }

        public override void addOutput(Pipe output)
        {
            throw new System.NotImplementedException();
        }

        public override void addTool(Tool tool)
        {
            throw new System.NotImplementedException();
        }

        public override ToolContainer getContainer(Pipe input)
        {
            throw new System.NotImplementedException();
        }
    }
}