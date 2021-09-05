using System;
using analysis_engine.Util.Factories;

;
namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public class PipelineBuilderDirector
    {
        private PipeFactory _pipeFactory;
        private PipelineBuilder _pipelineBuilder;

        public PipelineBuilderDirector(PipelineBuilder pipelineBuilder)
        {
            _pipelineBuilder = pipelineBuilder;
        }

        public Pipeline Construct(String pipeline)
        {
            _pipelineBuilder.BuildPipeline();
            _pipelineBuilder.BuildDrain();
            _pipelineBuilder.BuildSource();
            _pipelineBuilder.BuildFilters(pipeline);
            return _pipelineBuilder.GetPipeline();
        }
    }
}