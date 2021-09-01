using System.Collections.Generic;
using analysis_engine.Util;

namespace analysis_engine.Filter
{
    public class Filter
    {
        public List<ToolContainer> Tools { get; set; }
        public Pipe input;
        public Pipe output;

        public void update(int frameTime)
        {
        }

    }
}