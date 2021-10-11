namespace analysis_engine
{
    public class InputMergerPipeFactory : PipeFactory
    {
        public int NumberOfInputs { get; set; }

        public InputMergerPipeFactory(int numberOfInputs)
        {
            NumberOfInputs = numberOfInputs;
        }

        public override Pipe GetPipe()
        {
            return new InputMerger(NumberOfInputs);
        }
    }
}