namespace analysis_engine
{
    /**
     * Description:
     * ->This is a concrete implementation of the Pipeline abstract class.
     * A linear pipeline has a single channel of pipes and filters which data moves through to be analyzed.
     */
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