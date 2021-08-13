using src.AnalysisTools;
using src.AnalysisTools.ConcreteTools;

namespace src.Subsystems.Analysis
{
    public class AnalysisModels: IAnalysisModels
    {
        public ITool AnimalRecognition { get; }
        public ITool VehicleRecognition { get; }
        public ITool PersonRecognition { get;  }
        
        public AnalysisModels()
        {
            AnimalRecognition = new AnimalRecognition();
            VehicleRecognition = new CarRecognition();
            PersonRecognition = new PersonRecognition();
        }
    }
}