
namespace analysis_engine
{
    public class FilterBuilder
    {
        private Filter _filter;
        private FilterManager _filterManager;
        public Filter GetFilter()
        {
            return _filter;
        }

        public void BuildToolContainer(string toolContainer, bool last)
        {
            ToolContainerBuilder _toolContainerBuilder;
            PipeFactory mergerPipeFactory = new ConcurrentInputMergerPipeFactory();
            string[] containerInfo = toolContainer.Split(':');
            switch (containerInfo[0])
            {
                case "analysis":
                    _toolContainerBuilder = new AnalysisToolContainerBuilder();
                    break;
                case "drawing":
                    _toolContainerBuilder = new DrawingToolContainerBuilder();
                    break;
                case "drone":
                    _toolContainerBuilder = new DroneToolContainerBuilder();
                    break;
                case "dynamic":
                    _toolContainerBuilder = new DynamicToolContainerBuilder();
                    break;
                default:
                    _toolContainerBuilder = new AnalysisToolContainerBuilder();
                    break;
            }
            
            _toolContainerBuilder.buildContainer(last);
            _toolContainerBuilder.addInput(_filter.Input);
            _toolContainerBuilder.addOutput(_filter.Output);
            _toolContainerBuilder.addTool(containerInfo[1]);
            _filter.Tools.Add(_toolContainerBuilder.getContainer());
        }

        public void BuildFilterManager(string filterManager)
        {
            switch (filterManager)
            {
                case "concurrency":
                    _filterManager = new ConcurrencyManager(_filter);
                    break;
            }
        }

        public void AddInput(Pipe input)
        {
            _filter.Input = input;
        }

        public void AddOutput(Pipe output)
        {
            _filter.Output = output;
        }

        public void BuildFilter()
        {
            _filter = new Filter();
        }
    }
}