namespace analysis_engine.Util.Factories
{
    public class InputSplitterPipeFactory : PipeFactory
    {
        public override Pipe getPipe()
        {
            return new InputSplitter();
        }
    }
}