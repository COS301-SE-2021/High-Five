using System.Collections.Generic;

namespace src.AnalysisTools.AnalysisThread
{
    public struct
        AnalysisOutput //This struct is specifically for the output of CarRecognition, PersonRecognition and AnimalRecognition
    {
        public string Purpose;
        public List<float> Boxes;
        public List<string> Classes;
    }
}