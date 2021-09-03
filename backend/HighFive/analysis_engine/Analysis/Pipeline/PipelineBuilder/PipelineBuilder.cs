using analysis_engine.Util;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public abstract class PipelineBuilder
    {
        public Pipeline Pipeline { get; set; }

        public PipelineBuilder()
        {
        }

        public abstract void BuildPipeline();
        public abstract void AddSource(Pipe source);
        public abstract void AddDrain(Pipe drain);
    }
}