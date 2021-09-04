using System.Threading.Tasks;
using analysis_engine.Analysis.Util.Data;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Pipes
{
    public class ConcurrentInputMergerPipe : Pipe
    {
        private Pipe _output;
        private Buffer buffer;
        public ConcurrentInputMergerPipe()
        {
            buffer = new RingBuffer(5000);
        }

        public void SetOutput(Pipe output)
        {
            _output = output;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _output.Push(buffer.Pop());
                }
            });
        }

        public void Push(Data.Data data)
        {
            buffer.Push(data);
        }

        public Data.Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}