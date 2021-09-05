using System.Collections.Concurrent;

namespace analysis_engine
{
    public class DataPool
    {
        private int _capacity;
        private int _checkedOut;
        private DataFactory _factory;
        public ConcurrentQueue<Data> IdleQueue;
        public DataPool(int capacity, DataFactory factory)
        {
            _capacity = capacity;
            _checkedOut = 0;
            _factory = factory;
            IdleQueue = new ConcurrentQueue<Data>();
            for (var i = 0; i < capacity; i++)
            {
                IdleQueue.Enqueue(_factory.MakeData());
            }
        }

        private void Resize(bool increase)
        {
            if (increase)
            {
                for (var i = 0; i < _capacity; i++)
                {
                    IdleQueue.Enqueue(_factory.MakeData());
                }
                _capacity *= 2;
            }
            else
            {
                //TODO implement some way of reducing the size of the queue by half
            }
        }


        public void ReleaseData(Data data)
        {
            if (IdleQueue.Count >= _capacity * 0.75)
            {
                Resize(false);
            }
            else
            {
                IdleQueue.Enqueue(data);
            }
        }

        public Data GetData()
        {
            Data result;
            bool success = IdleQueue.TryDequeue(out result);
            if (success)
            {
                return result;
            }
            else
            {
                Resize(true);
                IdleQueue.TryDequeue(out result);
                return result;
            }
        }

    }

   
}