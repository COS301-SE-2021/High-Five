namespace analysis_engine
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