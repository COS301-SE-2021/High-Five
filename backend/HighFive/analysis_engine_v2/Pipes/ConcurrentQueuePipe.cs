using System.Collections.Concurrent;
using High5SDK;

namespace analysis_engine
{
    public class ConcurrentQueuePipe : Pipe
    {
        private BlockingCollection<Data> dataQueue;

        public ConcurrentQueuePipe()
        {
            dataQueue = new BlockingCollection<Data>(new ConcurrentQueue<Data>());
        }

        public void Push(Data data)
        {
            dataQueue.Add(data);
        }

        public Data Pop()
        {
            var item = dataQueue.Take();
            return item;
        }
    }
}