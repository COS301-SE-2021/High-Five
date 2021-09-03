using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data.ConcreteData;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Data
{
    public class Data
    {
        
        public static int Count { get; set; } = 0;
        
        public int Id { get; set; }
        
        public DataPool Pool { get; set; }
        
        public Frame Frame { get; set; }

        public List<MetaData> Meta { get; set; }

        
        public Data clone()
        {
            Data temp = Pool.GetData();
            temp.Frame.Image = Frame.Image;
            temp.Frame.FrameID = Frame.FrameID;
            temp.Meta.Clear();
            return temp;
        }

        public Data()
        {
            Frame = new Frame();
            Meta = new List<MetaData>();
            Id = Count++;
        }

        public Data(Frame frame)
        {
            Frame = frame;
        }
    }
}