using System.Collections.Generic;

namespace High5SDK
{
    public class Data
    {

        public static int Count { get; set; } = 0;

        public int Id { get; set; }

        public DataPool Pool { get; set; }

        public Frame Frame { get; set; }

        public List<MetaData> Meta { get; set; }

        public Data Clone()
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