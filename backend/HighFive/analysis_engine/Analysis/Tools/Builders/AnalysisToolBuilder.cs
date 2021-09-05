using analysis_engine.Analysis.Tools.AnalysisTools;
using analysis_engine.Tools;

namespace analysis_engine.Analysis.Tools.Builders
{
    public class AnalysisToolBuilder : ToolBuilder
    {
        public override void BuildTool(string name)
        {
            switch (name)
            {
                case "people":
                    Tool = new PersonRecognitionTool();
                    break;
                case "people-1":
                    Tool = new SelfDrawingPersonRecognitionTool();
                    break;
                case "animal":
                    Tool = new AnimalRecognitionTool();
                    break;
                case "animals-1":
                    Tool = new SelfDrawingAnimalRecognitionTool();
                    break;
                case "vehicle":
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