using System.Threading;
using analysis_engine.Tools;
using analysis_engine.Util;

namespace analysis_engine.Filter
{
    public class ToolContainer
    {
        private Tool _tool;
        private Pipe _input;
        private Pipe _output;
        private FilterManager _manager;
        
        public ToolContainer(Tool tool, Pipe input, Pipe output, FilterManager manager)
        {
            _tool = tool;
            _input = input;
            _output = output;
            _manager = manager;
            tool.Init();
        }
    }
}