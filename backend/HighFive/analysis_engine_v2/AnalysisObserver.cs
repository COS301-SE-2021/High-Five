namespace analysis_engine
{
    public class AnalysisObserver
    {
        public bool Done;
        public AnalysisObserver(string url)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", "analysis:fastvehicles,drawing:boxes");
            manager.GiveLinkToFootage("video", url, "");
            manager.StartAnalysis();
            Done = false;
        }

        public void AnalysisFinished()
        {
            Done = true;
        }
    }
}