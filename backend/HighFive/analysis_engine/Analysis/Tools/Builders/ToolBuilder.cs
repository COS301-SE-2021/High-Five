using System;

namespace analysis_engine.Tools.Builders
{
    public abstract class ToolBuilder
    {
        public abstract void buildTool(String name);
        public abstract Tool getTool();
    }
}