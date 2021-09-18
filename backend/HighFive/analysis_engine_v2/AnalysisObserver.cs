using System;
using System.Diagnostics;
using System.IO;

namespace analysis_engine
{
    public class AnalysisObserver
    {
        public volatile bool Done;
        private Stopwatch _watch;
        public AnalysisObserver(string url, string mediaType, string pipelineString, string outputurl)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", pipelineString, mediaType, outputurl);
            manager.GiveLinkToFootage(mediaType, url);
            _watch = new Stopwatch();
            _watch.Reset();
            _watch.Start();
            manager.StartAnalysis();
            Done = false;
        }
        public AnalysisObserver(Stream input, string mediaType, string pipelineString, string outputurl)
        {
            var manager = new Manager(this);
            manager.CreatePipeline("linear", pipelineString, mediaType, outputurl);
            manager.GiveLinkToFootage(mediaType, "", input);
            manager.StartAnalysis();
            Done = false;
        }

        public void AnalysisFinished(int frameCount)
        {
            _watch.Stop();
            Console.WriteLine("Average Throughput: "+_watch.ElapsedMilliseconds/Convert.ToDouble(frameCount)+"ms/frame");
            Done = true;
        }
    }
}