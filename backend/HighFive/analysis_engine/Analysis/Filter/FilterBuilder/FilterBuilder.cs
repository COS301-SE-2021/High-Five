using analysis_engine.Util;

namespace analysis_engine.Filter.FilterBuilder
{
    public class FilterBuilder
    {
        private Filter _filter;

        public Filter GetFilter()
        {
            return _filter;
        }

        public void BuildToolContainer(string toolContainer)
        {
            
        }

        public void AddInput(Pipe input)
        {
            _filter.Input = input;
        }

        public void AddOutput(Pipe output)
        {
            _filter.Output = output;
        }

        public void BuildFilter()
        {
            _filter = new Filter();
        }
    }
}