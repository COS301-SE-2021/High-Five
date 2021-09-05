using analysis_engine.Analysis.Filter.ToolContainerBuilder;
using analysis_engine.Analysis.Util.Pipes;
using analysis_engine.Util;
using analysis_engine.Util.Factories;

namespace analysis_engine.Filter.FilterBuilder
{
    public class FilterBuilder
    {
        private Filter _filter;
        private FilterManager _filterManager;
        public Filter GetFilter()
        {
            return _filter;
        }

        public void BuildToolContainer(string toolContainer)
        {
            ToolContainerBuilder.ToolContainerBuilder _toolContainerBuilder;
            PipeFactory mergerPipeFactory = new ConcurrentInputMergerPipeFactory();
            string[] containerInfo = toolContainer.Split(":");
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
                default:
                    _toolContainerBuilder = new AnalysisToolContainerBuilder();
                    break;
            }
            //Create the input merger 
            _filter.ConcurrentInputMerger = mergerPipeFactory.getPipe();
            ConcurrentInputMergerPipe temp = (ConcurrentInputMergerPipe) _filter.ConcurrentInputMerger;
            temp.SetOutput(_filter.Output);
            
            _toolContainerBuilder.buildContainer();
            _toolContainerBuilder.addInput(_filter.Input);
            _toolContainerBuilder.addOutput(_filter.ConcurrentInputMerger);
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