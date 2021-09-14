namespace analysis_engine
{
    public class AnalysisObserver
    {
        public bool Done;
        public AnalysisObserver(string url, string mediaType, string pipelineString)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", pipelineString);
            manager.GiveLinkToFootage(mediaType, url, "");
            manager.StartAnalysis();
            Done = false;
        }

        public void AnalysisFinished()
        {
            Done = true;
        }
    }
}