using System.IO;

namespace analysis_engine
{
    public class AnalysisObserver
    {
        public bool Done;
        public AnalysisObserver(string url, string mediaType, string pipelineString, string outputurl)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", pipelineString);
            manager.GiveLinkToFootage(mediaType, url, outputurl);
            manager.StartAnalysis();
            Done = false;
        }
        public AnalysisObserver(Stream input, string mediaType, string pipelineString, string outputurl)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", pipelineString);
            manager.GiveLinkToFootage(mediaType, "", outputurl, input);
            manager.StartAnalysis();
            Done = false;
        }

        public void AnalysisFinished()
        {
            Done = true;
        }
    }
}