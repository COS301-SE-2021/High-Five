
using System.Collections;
using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data;



namespace analysis_engine.Util
{
    public class DataPool
    {
        private int _capacity;
        private DataFactory _factory;
        public Queue<Data> IdleQueue;
        public DataPool(int capacity, DataFactory factory)
        {
            _capacity = capacity;
            _factory = factory;
            IdleQueue = new Queue<Data>();
            for (var i = 0; i < capacity; i++)
            {
                IdleQueue.Enqueue(_factory.makeData());
            }
        }

        private void Resize(bool increase)
        {
            if (increase)
            {
                for (var i = 0; i < _capacity; i++)
                {
                    IdleQueue.Enqueue(_factory.makeData());
                }
                _capacity *= 2;
            }
            else
            {
                //TODO implement some way of reducing the size of the queue by half
            }
        }


        public void ReturnData(Data data)
        {
            IdleQueue.Enqueue(data);
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
                return IdleQueue.Dequeue();
            }
        }

    }

   
}