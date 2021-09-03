﻿using analysis_engine.Util;

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
            throw new System.NotImplementedException();
        }

        public override void AddDrain(Pipe drain)
        {
            throw new System.NotImplementedException();
        }
    }
}