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
            //TODO dynamic construction function implementation
            return makeDemoPipeline();
        }

        private Pipeline makeDemoPipeline()
        {
            _pipelineBuilder.BuildPipeline();
            return _pipelineBuilder.GetPipeline();
        }
    }
}