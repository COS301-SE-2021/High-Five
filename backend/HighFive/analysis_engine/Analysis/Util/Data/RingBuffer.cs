using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Data
{
    public class RingBuffer: Buffer
    {
        protected Data[] _ring;
        
        public RingBuffer(long size) : base(size)
        {
            _ring = new Data[size];
        }

        public override void Push(Data data)
        {
            throw new System.NotImplementedException();
        }

        public override Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}