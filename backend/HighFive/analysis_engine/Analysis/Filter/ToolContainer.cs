using System.Threading;
using analysis_engine.Tools;
using analysis_engine.Util;

namespace analysis_engine.Filter
{
    public abstract class ToolContainer
    {
        public Tool Tool { get; set; }
        public Pipe Input { get; set; }
        public Pipe Output { get; set; }

        public FilterManager Manager { get; set; }
        
        public ToolContainer(Tool tool, Pipe input, Pipe output, FilterManager manager)
        {
            Tool = tool; 
            Input = input;
            Output = output;
            Manager = manager;
            tool.Init();
        }

        protected ToolContainer()
        {
        }
    }
}