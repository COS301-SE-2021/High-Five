using System;

namespace analysis_engine
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