using System.Collections.Generic;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Pipeline
{
    public abstract class Pipeline
    {
        public List<analysis_engine.Filter.Filter> Filters { get; set; }
        public Pipe Source { get; set; }
        public Pipe Drain { get; set; }
        public abstract void Init();
    }
}