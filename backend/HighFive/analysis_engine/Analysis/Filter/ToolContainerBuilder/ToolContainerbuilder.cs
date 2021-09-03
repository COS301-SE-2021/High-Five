using System;
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
        public abstract void addTool(String toolName);
        public abstract ToolContainer getContainer();
    }
}