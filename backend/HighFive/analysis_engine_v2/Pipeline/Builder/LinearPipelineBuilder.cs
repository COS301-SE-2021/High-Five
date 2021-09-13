
namespace analysis_engine
{
    public class LinearPipelineBuilder : PipelineBuilder
    {
        public LinearPipelineBuilder()
        {
            _pipeFactories = new PipeFactory[1];
            _pipeFactories[0] = new ConcurrentQueuePipeFactory();
        }

        public override void BuildPipeline()
        {
            Pipeline = new LinearPipeline();
        }

        public override void BuildSource()
        {
            Pipeline.Source = _pipeFactories[0].GetPipe();
        }

        public override void BuildDrain()
        {
            Pipeline.Drain = _pipeFactories[0].GetPipe();
        }

        public override void BuildFilters(string filterString)
        {
            string[] filterStrings = filterString.Split(',');
            Filter[] temp = new Filter[filterStrings.Length];
            int count = 0;
            foreach (var s in filterStrings)
            {
                _filterBuilder.BuildFilter();
                
                _filterBuilder.BuildFilterManager("concurrency");

                if (count == 0)//If this is the first filter in the pipeline
                {
                    _filterBuilder.AddInput(Pipeline.Source);
                    _filterBuilder.AddOutput(_pipeFactories[0].GetPipe());
                }else if (count == filterStrings.Length - 1)//else if this is the last filter in the pipeline
                {
                    _filterBuilder.AddInput(temp[count-1].Output);
                    _filterBuilder.AddOutput(Pipeline.Drain);
                }
                else//else this is just an internal filter
                {
                    _filterBuilder.AddInput(temp[count-1].Output);
                    _filterBuilder.AddOutput(_pipeFactories[0].GetPipe());
                }
                _filterBuilder.BuildToolContainer(s, filterStrings.Length==(count-1));
                temp[count] = _filterBuilder.GetFilter();
                count++;
            }
        }

        public override Pipeline GetPipeline()
        {
            return Pipeline;
        }
    }
}