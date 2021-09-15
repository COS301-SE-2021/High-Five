namespace analysis_engine
{
    public abstract class Buffer
    {
        protected long _size;
        public long Head { get; set; }

        protected Buffer(long size)
        {
            _size = size;
            Head = 0;
        }

        public abstract void Push(Data data);
        public abstract Data Pop();
        
    }
}