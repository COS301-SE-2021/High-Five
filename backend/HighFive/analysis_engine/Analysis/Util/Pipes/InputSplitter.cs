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

        public void push(Data data)
        {
            foreach (var input in _inputs)
            {
                input.push(data);
            }
        }

        public Data pop()
        {
            throw new System.NotImplementedException();
        }
    }
}