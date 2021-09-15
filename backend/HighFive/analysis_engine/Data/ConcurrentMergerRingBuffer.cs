namespace analysis_engine
{
    public class ConcurrentMergerRingBuffer : RingBuffer
    {
        public ConcurrentMergerRingBuffer(long size) : base(size)
        {
        }

        /*
         * This is a blocking function, it will keep waiting until
         * the Data object that contains the next FrameID is inserted into the buffer.
         */
        public override Data Pop()
        {
            Head = Head % _size;
            while (_ring[Head] == null){}
            Data temp = _ring[Head];
            _ring[Head] = null;
            Head++;
            return temp;
        }
    }
}