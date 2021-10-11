
using System;
using System.Collections.Generic;

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
            Console.WriteLine("Source: "+((ConcurrentQueuePipe)Pipeline.Source)._label );
        }

        public override void BuildDrain()
        {
            Pipeline.Drain = _pipeFactories[0].GetPipe();
            Console.WriteLine("Drain: "+((ConcurrentQueuePipe)Pipeline.Drain)._label );
        }

        public override void BuildFilters(string filterString)
        {
            string[] filterStrings = filterString.Split(',');
            List<Filter> temp = new List<Filter>();
            int count = 0;
            foreach (var s in filterStrings)
            {
                _filterBuilder.BuildFilter();
                
                _filterBuilder.BuildFilterManager("concurrency");

                if (filterStrings.Length == 1)
                {
                    _filterBuilder.AddInput(Pipeline.Source);
                    _filterBuilder.AddOutput(Pipeline.Drain);
                }else if (count == 0)//If this is the first filter in the pipeline
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
                _filterBuilder.BuildToolContainer(s, filterStrings.Length==(count+1));
                temp.Add(_filterBuilder.GetFilter());
                count++;
            }

            Pipeline.Filters = temp;
        }

        public override Pipeline GetPipeline()
        {
            return Pipeline;
        }
    }
}