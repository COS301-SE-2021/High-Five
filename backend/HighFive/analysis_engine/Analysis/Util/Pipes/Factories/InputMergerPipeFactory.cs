using analysis_engine.Analysis.Util.Pipes;

namespace analysis_engine.Util.Factories
{
    public class InputMergerPipeFactory : PipeFactory
    {
        public int NumberOfInputs { get; set; }

        public InputMergerPipeFactory(int numberOfInputs)
        {
            NumberOfInputs = numberOfInputs;
        }

        public override Pipe getPipe()
        {
            return new InputMerger(NumberOfInputs);
        }
    }
}