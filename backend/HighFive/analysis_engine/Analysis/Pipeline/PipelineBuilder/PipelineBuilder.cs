using analysis_engine.Filter.FilterBuilder;
using analysis_engine.Util.Factories;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public abstract class PipelineBuilder
    {
        protected PipeFactory[] _pipeFactories;

        protected FilterBuilder _filterBuilder;

        public Pipeline Pipeline { get; set; }

        public PipelineBuilder()
        {
            _filterBuilder = new FilterBuilder();
        }

        public abstract void BuildPipeline();
        public abstract void BuildSource();
        public abstract void BuildDrain();
        public abstract void BuildFilters(string filterString);
        public abstract Pipeline GetPipeline();
    }
}