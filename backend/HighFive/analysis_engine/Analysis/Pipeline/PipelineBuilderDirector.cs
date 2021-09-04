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
            _pipelineBuilder = pipelineBuilder;
            _pipeFactories = new PipeFactory[5];
            _toolBuilders = new ToolBuilder[3];
            _toolContainerBuilders = new ToolContainerBuilder[3];
            _filterBuilder = new FilterBuilder();
            _pipelineBuilder = pipelineBuilder;
        }

        public void Construct(String pipeline)
        {
        }
    }
}