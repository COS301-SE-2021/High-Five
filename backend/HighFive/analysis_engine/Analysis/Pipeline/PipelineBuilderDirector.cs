using System;
using analysis_engine.Analysis.Tools.Builders;
using analysis_engine.Filter.FilterBuilder;
using analysis_engine.Filter.ToolContainerBuilder;
using analysis_engine.Util.Factories;
;
namespace analysis_engine.Analysis.Pipeline
{
    public class PipelineBuilderDirector
    {
        private PipeFactory[] _pipeFactories;
        private ToolBuilder[] _toolBuilders;
        private ToolContainerBuilder[] _toolContainerBuilders;
        private FilterBuilder _filterBuilder;
        private PipelineBuilder.PipelineBuilder _pipelineBuilder;

        public PipelineBuilderDirector(PipelineBuilder.PipelineBuilder pipelineBuilder)
        {
        }

        public void Construct(String pipeline)
        {
        }
    }
}