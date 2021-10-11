using System.Threading.Tasks;
using High5SDK;

namespace analysis_engine
{
    /*
         *      Description:
         * This is a concrete implementation of the Pipe interface.
         * It acts as a buffer which receives frames in no particular order and then
         * outputs the frames in their correct order.
         * It achieves this by using a ring buffer which maps frames to the buffer according to their frameID. 
         *      Members:
         * -> _output: The pipe where the correctly ordered frames need to be output to
         * -> buffer: The ring buffer which backs this Pipe implementation
         */
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