using analysis_engine.Util.Factories;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
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
            Pipeline.Source = _pipeFactories[0].getPipe();
        }

        public override void BuildDrain()
        {
            Pipeline.Drain = _pipeFactories[0].getPipe();
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