using High5SDK;

namespace analysis_engine
{
    public class DrawingToolBuilder : ToolBuilder
    {
        public override void BuildTool(string name)
        {
            switch (name)
            {
                case "boxes":
                    Tool = new BoxDrawingTool();
                    break;
                case "labels":
                    Tool = new LabelDrawingTool();
                    break;
            }
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}