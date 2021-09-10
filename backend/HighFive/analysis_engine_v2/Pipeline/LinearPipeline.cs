﻿namespace analysis_engine
{
    public class LinearPipeline : Pipeline
    {
        public override Filter Init()
        {
            foreach (var filter in Filters)
            {
                filter.start();
            }

            return Filters[Filters.Count - 1];
        }
    }
}