using System.Collections.Concurrent;
using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Util
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