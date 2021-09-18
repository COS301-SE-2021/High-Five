using System.Collections.Generic;

namespace analysis_engine
{
    /**
     * Description: This abstract class is a representation of a complete analysis pipeline with source and drain pipes and a list of filters.
     * Members:
     * -> Source: This is the data entry point of the pipeline. Data objects that need to be analyzed are pushed in here.
     * -> Drain: This is the data exit point of the pipeline. Analyzed data is popped from this pipe.
     * -> Filters: This is a list containing all the filters of the pipeline. 
     */
    public abstract class Pipeline
    {
        public List<Filter> Filters { get; set; }
        public Pipe Source { get; set; }
        public Pipe Drain { get; set; }
        public abstract Filter Init();
    }
}