using src.AnalysisTools;
using src.AnalysisTools.ConcreteTools;

namespace src.Subsystems.Analysis
{
    public class AnalysisModels: IAnalysisModels
    {
        private readonly ITool _animalRecognition;
        private readonly ITool _vehicleRecognition;
        private readonly ITool _personRecognition;
        
        public AnalysisModels()
        {
            _animalRecognition = new AnimalRecognition();
            _vehicleRecognition = new CarRecognition();
            _personRecognition = new PersonRecognition();
        }

        public ITool GetTool(string toolName)
        {
            switch (toolName)
            {
                case "AnimalCounting":
                    return _animalRecognition;
                case "VehicleRecognition":
                    return _vehicleRecognition;
                case "PersonRecognition":
                    return _personRecognition;
                default:
                    return null;
            }
        }
    }
}