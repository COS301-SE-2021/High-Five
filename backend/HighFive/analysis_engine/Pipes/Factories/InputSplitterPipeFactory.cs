namespace analysis_engine
{
    public class InputSplitterPipeFactory : PipeFactory
    {
        public override Pipe GetPipe()
        {
            return new InputSplitter();
        }
    }
}