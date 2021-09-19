using System.Collections.Generic;
using High5SDK;

namespace analysis_engine
{
    public class BoxCoordinateData : MetaData
    {
        public string PredictedClass;
        public List<float> Boxes;
        public List<string> Classes;
    }
}