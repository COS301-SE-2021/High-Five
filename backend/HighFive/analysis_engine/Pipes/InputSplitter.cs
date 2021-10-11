using System.Collections.Generic;
using High5SDK;

namespace analysis_engine
{
    public class InputSplitter : Pipe
    {
        private List<Pipe> _inputs;
        
        public InputSplitter()
        {
            _inputs = new List<Pipe>();
        }

        public void AddInput(Pipe inputPipe)
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