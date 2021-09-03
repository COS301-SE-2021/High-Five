using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data.ConcreteData;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Data
{
    public class Data
    {
        public Frame Frame { get; set; }
        public bool HasFrame { get; set; }
        
        public List<MetaData> Meta { get; set; }
        public Data()
        {
            Frame = new Frame();
            Meta = null;
        }

        public Data(Frame frame, bool hasFrame)
        {
            Frame = frame;
            HasFrame = hasFrame;
        }
    }
}