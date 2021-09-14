using System;

namespace analysis_engine
{
    public abstract class ToolContainerBuilder
    {
        protected ToolContainer _toolContainer;
        public abstract void buildContainer(bool last);
        public abstract void addInput(Pipe input);
        public abstract void addOutput(Pipe output);
        public abstract void addTool(String toolName);
        public abstract ToolContainer getContainer();
    }
}