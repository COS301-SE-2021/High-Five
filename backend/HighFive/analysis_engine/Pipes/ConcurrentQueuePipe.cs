using System.Collections.Concurrent;

namespace analysis_engine
{
    public class ConcurrentQueuePipe : Pipe
    {
        private ConcurrentQueue<Data> dataQueue;

        public ConcurrentQueuePipe()
        {
            dataQueue = new ConcurrentQueue<Data>();
        }

        public void Push(Data data)
        {
            dataQueue.Enqueue(data);
        }

        public Data Pop()
        {
            Data item;
            var isSuccessful = dataQueue.TryDequeue(out item);
            return isSuccessful ? item : null;
        }
    }
}