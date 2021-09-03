using analysis_engine.Analysis.Tools;
using analysis_engine.Tools;
using analysis_engine.Util;

namespace analysis_engine.Filter.ToolContainerBuilder
{
    public abstract class ToolContainerBuilder
    {
        protected ToolContainer _toolContainer;
        public abstract void buildContainer();
        public abstract void addInput(Pipe input);
        public abstract void addOutput(Pipe output);
        public abstract void addTool(Tool tool);
        public abstract ToolContainer getContainer(Pipe input);
    }
}