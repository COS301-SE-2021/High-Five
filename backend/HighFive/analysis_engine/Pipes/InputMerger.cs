using System.Collections.Concurrent;
using System.Linq;
using High5SDK;

namespace analysis_engine
{
    public class InputMerger : Pipe
    {
        private Pipe _output;
        private ConcurrentDictionary<int, Data> _finalData;
        private int _numPipelines;
        private ConcurrentDictionary<int, int> _piplineCountdowns;
        public InputMerger(int numPipelines)
        {
            this._numPipelines = numPipelines;
            _finalData = new ConcurrentDictionary<int, Data>();
            _piplineCountdowns = new ConcurrentDictionary<int, int>();
        }

        public void SetOutput(Pipe output)
        {
            _output = output;
        }

        public void Push(Data data)
        {
            if (_finalData.TryAdd(data.Frame.FrameID, data))
            {
                _piplineCountdowns.TryAdd(data.Frame.FrameID, _numPipelines-1);
            }
            else
            {
                _finalData[data.Frame.FrameID].Meta = _finalData[data.Frame.FrameID].Meta.Concat(data.Meta).ToList();
                _piplineCountdowns[data.Frame.FrameID]--;
            }

            if (_piplineCountdowns[data.Frame.FrameID] == 0)
            {
                Data output;
                _finalData.TryRemove(data.Frame.FrameID, out output);
                _output.Push(output);
            }
        }

        public Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}