using analysis_engine.Util;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public class LinearPipelineBuilder : PipelineBuilder
    {
        public override void BuildPipeline()
        {
            Pipeline = new LinearPipeline();
        }

        public override void AddSource(Pipe source)
        {
            Pipeline.Source = source;
        }

        public override void AddDrain(Pipe drain)
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