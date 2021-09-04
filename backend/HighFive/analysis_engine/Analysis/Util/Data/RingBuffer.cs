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
            long index = data.Frame.FrameID % _size;
            _ring[index] = data;
        }
//TODO Does it make sense for this buffer to have a Pop() method?
        public override Data Pop()
        {
            Head = Head % _size;
            return _ring[Head++];
        }
    }
}