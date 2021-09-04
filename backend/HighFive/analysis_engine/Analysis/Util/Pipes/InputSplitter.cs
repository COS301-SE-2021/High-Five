using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Util
{
    public class InputSplitter : Pipe
    {
        private List<Pipe> _inputs;
        
        public InputSplitter()
        {
            _inputs = new List<Pipe>();
        }

        public void addInput(Pipe inputPipe)
        {
            _inputs.Add(inputPipe);
        }

        public void Push(Data data)
        {
            foreach (var input in _inputs)
            {
                input.Push(data.Clone());
            }
            data.Pool.ReleaseData(data);
        }

        public Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}