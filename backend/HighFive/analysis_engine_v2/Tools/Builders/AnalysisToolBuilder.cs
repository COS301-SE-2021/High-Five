using High5SDK;

namespace analysis_engine
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
                case "vehicles":
                    Tool = new VehicleRecognitionTool();
                    break;
                case "vehicles-1":
                    Tool = new SelfDrawingAnimalRecognitionTool();
                    break;
                case "fastvehicles":
                    Tool = new FastVehicleRecognitionTool();
                    break;
                case "fastpeople":
                    Tool = null;
                    break;
                case "gender":
                    Tool = new GenderDetectionTool();
                    break;
                case "age":
                    Tool = new AgeDetectionTool();
                    break;
                case "segmentation":
                    Tool = new ImageSegmentationTool();
                    break;
                case "upscale":
                    Tool = new UpscaleImageTool();
                    break;
            }
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}