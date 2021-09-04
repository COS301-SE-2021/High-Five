using analysis_engine.Util;
using analysis_engine.Util.Factories;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public class ParallelPipelineBuilder : PipelineBuilder
    {
        public ParallelPipelineBuilder(int channels)
        {
            _pipeFactories = new PipeFactory[3];
            _pipeFactories[0] = new ConcurrentQueuePipeFactory();
            _pipeFactories[1] = new InputSplitterPipeFactory();
            _pipeFactories[2] = new InputMergerPipeFactory(3);
        }

        public override void BuildPipeline()
        {
            Pipeline = new ParallelPipeline();
        }

        public override void BuildSource(Pipe source)
        {
            Pipeline.Source = source;
        }

        public override void BuildDrain(Pipe drain)
        {
            Pipeline.Drain = drain;
        }

        public override void AddFilter(analysis_engine.Filter.Filter filter)
        {
            Pipeline.Filters.Add(filter);
        }

        public override Pipeline GetPipeline()
        {
            return Pipeline;
        }
    }
}