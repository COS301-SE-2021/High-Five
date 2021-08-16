using src.AnalysisTools;
using src.AnalysisTools.ConcreteTools;

namespace src.Subsystems.Analysis
{
    public class AnalysisModels: IAnalysisModels
    {
        private readonly Tool _animalRecognition;
        private readonly Tool _vehicleRecognition;
        private readonly Tool _personRecognition;
        
        public AnalysisModels()
        {
            _animalRecognition = new AnimalRecognition();
            _vehicleRecognition = new CarRecognition();
            _personRecognition = new PersonRecognition();
        }

        public Tool GetTool(string toolName)
        {
            switch (toolName)
            {
                case "AnimalRecognition":
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