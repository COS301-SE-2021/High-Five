using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisService
    {
        public string AnalyzeImage(AnalyzeMediaRequest request);
        public string AnalyzeVideo(AnalyzeMediaRequest request);
    }
}