namespace analysis_engine.Filter
{
    public abstract class FilterManager
    {
        public Filter Filter;

        protected FilterManager(Filter filter)
        {
            Filter = filter;
        }

        public abstract void update();
    }
}