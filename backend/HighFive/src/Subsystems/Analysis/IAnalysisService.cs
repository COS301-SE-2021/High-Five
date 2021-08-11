using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisService
    {
        public string AnalyzeMedia(AnalyzeMediaRequest request);
    }
}