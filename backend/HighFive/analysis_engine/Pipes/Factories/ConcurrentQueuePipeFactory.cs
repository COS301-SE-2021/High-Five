namespace analysis_engine
{
    public class ConcurrentQueuePipeFactory : PipeFactory
    {
        private int _count;
        public ConcurrentQueuePipeFactory()
        {
            _count = 0;
        }
        public override Pipe GetPipe()
        {
            _count++;
            return new ConcurrentQueuePipe(_count);
        }
    }
}