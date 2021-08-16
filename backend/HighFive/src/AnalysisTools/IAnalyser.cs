using System.Collections.Generic;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    public interface IAnalyser
    {
        void StartAnalysis(Pipeline pipeline, IAnalysisModels analysisModels);

        void FeedFrame(byte[] frame);

        List<byte[]> GetFrames();

        void EndAnalysis();

    }
}