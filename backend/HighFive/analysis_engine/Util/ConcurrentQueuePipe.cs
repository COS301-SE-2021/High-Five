using System.Collections.Concurrent;

namespace analysis_engine.Util
{
    public class ConcurrentQueuePipe : Pipe
    {
        private ConcurrentQueue<Data> dataQueue;

        public ConcurrentQueuePipe()
        {
            dataQueue = new ConcurrentQueue<Data>();
        }

        public void push(Data data)
        {
            dataQueue.Enqueue(data);
        }

        public Data pop()
        {
            throw new System.NotImplementedException();
        }
    }
}