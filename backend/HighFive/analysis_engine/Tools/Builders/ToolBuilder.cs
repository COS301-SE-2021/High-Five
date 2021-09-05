using System;

namespace analysis_engine
{
    public abstract class ToolBuilder
    {
        public Tool Tool { get; set; }
        public abstract void BuildTool(String name);
        public abstract Tool GetTool();
    }
}