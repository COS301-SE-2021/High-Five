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
            }
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}