namespace analysis_engine
{
    public class ConcurrentQueuePipeFactory : PipeFactory
    {
        public override Pipe GetPipe()
        {
            return new ConcurrentQueuePipe();
        }
    }
}