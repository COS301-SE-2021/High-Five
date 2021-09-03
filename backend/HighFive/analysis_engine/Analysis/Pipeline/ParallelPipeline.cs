namespace analysis_engine.Analysis.Pipeline
{
    public class ParallelPipeline : Pipeline
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