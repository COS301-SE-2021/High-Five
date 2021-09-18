using System;
using System.Collections.Concurrent;
using High5SDK;

namespace analysis_engine
{
    public class ConcurrentQueuePipe : Pipe
    {
        private BlockingCollection<Data> dataQueue;
        public int _label;
        private int _warningSize;

        public ConcurrentQueuePipe(int label)
        {
            _label = label;
            _warningSize = 10;
            dataQueue = new BlockingCollection<Data>(new ConcurrentQueue<Data>());
        }

        public void Push(Data data)
        {
            dataQueue.Add(data);
            if (dataQueue.Count > _warningSize)
            {
                Console.WriteLine("Pipe "+_label+" size: "+dataQueue.Count);
                _warningSize *= 10;
            }
        }

        public Data Pop()
        {
            var item = dataQueue.Take();
            return item;
        }
    }
}