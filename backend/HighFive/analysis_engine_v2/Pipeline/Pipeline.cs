using System.Collections.Generic;

namespace analysis_engine
{
    public abstract class Pipeline
    {
        public List<Filter> Filters { get; set; }
        public Pipe Source { get; set; }
        public Pipe Drain { get; set; }
        public abstract void Init();
    }
}