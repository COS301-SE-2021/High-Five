using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Util
{
    public class InputSplitter : Pipe
    {
        private Pipe[] inputs;
        
        public InputSplitter(Pipe[] inputs)
        {
            this.inputs = inputs;
        }
        

        public void push(Data data)
        {
            foreach (var input in inputs)
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