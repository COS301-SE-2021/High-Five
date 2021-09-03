using System.Collections.Generic;
using System.Linq;

namespace analysis_engine.Analysis.Util.Data.ConcreteData
{
    public class BoxCoordinateData : MetaData
    {
        public string Purpose;
        public List<float> Boxes;
        public List<string> Classes;
    }
}