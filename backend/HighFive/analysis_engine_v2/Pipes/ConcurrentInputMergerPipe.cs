using System.Threading.Tasks;
using High5SDK;

namespace analysis_engine
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

        public void Push(Data data)
        {
            buffer.Push(data);
        }

        public Data Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}