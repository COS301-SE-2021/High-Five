using analysis_engine.Analysis.Tools.DrawingTools;
using analysis_engine.Tools;

namespace analysis_engine.Analysis.Tools.Builders
{
    public class DrawingToolBuilder : ToolBuilder
    {
        public override void BuildTool(string name)
        {
            switch (name)
            {
                case "box-0":
                    Tool = new BoxDrawingTool();
                    break;
            }
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}