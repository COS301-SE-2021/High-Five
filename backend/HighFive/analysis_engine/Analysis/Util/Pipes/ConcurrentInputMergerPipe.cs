using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Pipes
{
    public class ConcurrentInputMergerPipe : Pipe
    {
        private Pipe _output;

        public ConcurrentInputMergerPipe()
        {
        }

        public void SetOutput(Pipe output)
        {
            _output = output;
        }

        public void Push(Data.Data data)
        {
            throw new System.NotImplementedException();
        }

        public Data.Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}