namespace analysis_engine.Util.Factories
{
    public class ConcurrentQueuePipeFactory : PipeFactory
    {
        public override Pipe getPipe()
        {
            return new ConcurrentQueuePipe();
        }
    }
}