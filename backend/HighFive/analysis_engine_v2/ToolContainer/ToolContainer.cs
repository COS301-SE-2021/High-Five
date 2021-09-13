using System.Threading.Tasks;

namespace analysis_engine
{
    public abstract class ToolContainer
    {
        private bool _running;
        public Tool Tool { get; set; }
        public Pipe Input { get; set; }
        public Pipe Output { get; set; }

        public bool Last;
        public FilterManager Manager { get; set; }
        
        public ToolContainer(Tool tool, Pipe input, Pipe output, FilterManager manager, bool last)
        {
            Tool = tool; 
            Input = input;
            Output = output;
            Manager = manager;
            _running = false;
            tool.Init();
        }

        public void Start()
        {
            Tool.Init();
            _running = true;
            Task.Factory.StartNew(() =>
            {
                while (_running)
                {
                    Data temp = Input.Pop();
                    if (temp != null)
                    {
                        Output.Push(Tool.Process(temp));
                    }
                }
            });
        }

        public void Stop()
        {
            _running = false;
        }

        protected ToolContainer()
        {
        }
    }
}