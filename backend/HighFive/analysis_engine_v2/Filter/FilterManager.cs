namespace analysis_engine
{
    /**
     * This abstract class is used as an observer inside filters to manage multiple tool containers running concurrently.
     */
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