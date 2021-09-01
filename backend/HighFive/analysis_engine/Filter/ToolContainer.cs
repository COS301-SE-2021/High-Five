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
        public FilterManager Manager { get; set; }
        
        
        public ToolContainer(Tool tool, Pipe input, Pipe output)
        {
            _input = input;
            _output = output;
            _tool = tool;
            tool.Init();
        }
    }
}