using System.Collections.Concurrent;
using System.Collections.Generic;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    public class AnalyserImpl: IAnalyser
    {
        private List<BlockingCollection<byte[]>> _outputQueues;
        
        public void StartAnalysis(Pipeline pipeline, IAnalysisModels analysisModels)
        {
            _outputQueues = new List<BlockingCollection<byte[]>>();
            

        }

        public void FeedFrame(byte[] frame)
        {
            throw new System.NotImplementedException();
        }

        public List<byte[]> GetFrames()
        {
            throw new System.NotImplementedException();
        }

        public void EndAnalysis()
        {
            throw new System.NotImplementedException();
        }
    }
}