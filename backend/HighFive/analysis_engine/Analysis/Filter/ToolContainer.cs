using System.Threading;
using analysis_engine.Tools;
using analysis_engine.Util;

namespace analysis_engine.Filter
{
    public abstract class ToolContainer
    {
        protected Tool _tool;
        protected Pipe _input;
        protected Pipe _output;
        protected FilterManager _manager;
        
        public ToolContainer(Tool tool, Pipe input, Pipe output, FilterManager manager)
        {
            _tool = tool;
            _input = input;
            _output = output;
            _manager = manager;
            tool.Init();
        }

        protected ToolContainer()
        {
        }
    }
}