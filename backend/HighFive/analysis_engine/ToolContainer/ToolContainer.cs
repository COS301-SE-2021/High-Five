using System.Collections.Generic;
using System.Threading.Tasks;
using High5SDK;

namespace analysis_engine
{
    public abstract class ToolContainer
    {
        private bool _running;
        public Tool Tool { get; set; }
        public Pipe Input { get; set; }
        public Pipe Output { get; set; }

        public const int FrameSkipper = 5;
        
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
                if (Last)
                {
                    while (_running)
                    {
                        var temp = Input.Pop();
                        if (temp != null)
                        {
                            Output.Push(Tool.Process(temp));
                        }
                        else
                        {
                            Output.Push(null);
                            break;
                        }
                    }
                }
                else
                {
                    var count = 0;
                    Data data = null;
                    List<MetaData> meta = null;
                    while (_running)
                    {
                        if (count % FrameSkipper == 0)
                        {
                            var temp = Input.Pop();
                            if (temp == null)
                            {
                                Output.Push(null);
                                break;
                            }
                            data = Tool.Process(temp);
                            meta = data.Meta;
                            Output.Push(data);
                        }
                        else
                        {
                            var temp = Input.Pop();
                            if (temp == null)
                            {
                                Output.Push(null);
                                break;
                            }
                            data = temp;
                            data.Meta = meta;
                            Output.Push(data);
                        }

                        count++;
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