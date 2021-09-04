using System.Collections.Generic;

namespace analysis_engine.Analysis.Util.Data
{
    public class BoxCoordinateData : MetaData
    {
        public string Purpose;
        public List<float> Boxes;
        public List<string> Classes;
    }
}