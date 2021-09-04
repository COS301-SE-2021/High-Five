using analysis_engine.Util;
using analysis_engine.Util.Factories;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public abstract class PipelineBuilder
    {
        protected PipeFactory[] _pipeFactories;
        public Pipeline Pipeline { get; set; }

        public PipelineBuilder()
        {
            
        }

        public abstract void BuildPipeline();
        public abstract void BuildSource(Pipe source);
        public abstract void BuildDrain(Pipe drain);
        public abstract void AddFilter(analysis_engine.Filter.Filter filter);
        public abstract Pipeline GetPipeline();
    }
}