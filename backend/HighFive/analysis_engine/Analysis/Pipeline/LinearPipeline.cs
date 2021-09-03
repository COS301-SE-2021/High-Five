namespace analysis_engine.Analysis.Pipeline
{
    public class LinearPipeline : Pipeline
    {
        public override void Init()
        {
            foreach (var filter in Filters)
            {
                filter.start();
            }
        }
    }
}