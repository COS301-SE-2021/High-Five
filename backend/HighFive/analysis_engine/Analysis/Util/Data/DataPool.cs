using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Util
{
    public class DataPool
    {
        private int _capacity;
        private DataFactory _factory;
        public Queue<Data> ActivePool;
        public Queue<Data> IdlePool;
        
        public DataPool(int capacity, DataFactory factory)
        {
            _capacity = capacity;
            _factory = factory;
            IdlePool = new Queue<Data>();
            for (var i = 0; i < capacity; i++)
            {
                IdlePool.Enqueue(_factory.makeData());
            }
        }
        
    }
}