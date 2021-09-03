using analysis_engine.Analysis.Tools.ConcreteTools;
using analysis_engine.Tools;

namespace analysis_engine.Analysis.Tools.Builders
{
    public class AnalysisToolBuilder : ToolBuilder
    {
        public override void BuildTool(string name)
        {
            switch (name)
            {
                case "people-0":
                    Tool = new PersonRecognitionTool();
                    break;
                case "people-1":
                    Tool = new SelfDrawingPersonRecognitionTool();
                    break;
                case "animals-0":
                    Tool = new AnimalRecognitionTool();
                    break;
                case "animals-1":
                    Tool = new SelfDrawingAnimalRecognitionTool();
                    break;
                case "vehicles-0":
                    Tool = new VehicleRecognitionTool();
                    break;
                case "vehicles-1":
                    Tool = new SelfDrawingAnimalRecognitionTool();
                    break;
            }
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}