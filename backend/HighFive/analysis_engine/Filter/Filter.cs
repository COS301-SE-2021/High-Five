using System.Collections.Generic;

namespace analysis_engine
{
    public class Filter
    {
        public List<ToolContainer> Tools { get; set; }
        public Pipe Input { get; set; }
        public Pipe Output { get; set; }
        public Pipe ConcurrentInputMerger { get; set; }
        public Filter()
        {
            Tools = new List<ToolContainer>();
        }

        public void AddTool(ToolContainer tool)
        {
            Tools.Add(tool);
        }

        public void start()
        {
            foreach (var toolContainer in Tools)
            {
                toolContainer.Start();
            }
        }

        public void Update(int frameTime)
        {
        }

    }
}