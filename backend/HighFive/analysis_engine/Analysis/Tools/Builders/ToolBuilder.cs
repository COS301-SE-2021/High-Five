using System;
using analysis_engine.Tools;

namespace analysis_engine.Analysis.Tools.Builders
{
    public abstract class ToolBuilder
    {
        public Tool Tool { get; set; }
        public abstract void BuildTool(String name);
        public abstract Tool GetTool();
    }
}