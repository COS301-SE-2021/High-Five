using System.Collections.Generic;

namespace analysis_engine
{
    /**
     * This class is a container that represents the abstraction of a filter in a pipe filter pattern.
     * It hold a reference to an input and output pipe aswell as a list of the tool containers running inside the filter.
     * Additional provision has been made for concurrent tool containers by adding a reference to a concurrent input merger pipe
     * which acts as a normal pipe except that it will output frames in the correct order even if they are inserted in random order.
     */
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