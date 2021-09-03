using System.Linq;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Pipes
{
    public class InputMerger : Pipe
    {
        private Pipe _output;
        private Data.Data _finalData;
        public InputMerger(Pipe output)
        {
            this._output = output;
            _finalData = null;
        }

        public void push(Data.Data data)
        {
            if (_finalData == null)
            {
                _finalData = data;
            }
            else
            {
                _finalData.Meta=_finalData.Meta.Concat(data.Meta).ToList();
            }
        }

        public Data.Data pop()
        {
            throw new System.NotImplementedException();
        }
    }
}