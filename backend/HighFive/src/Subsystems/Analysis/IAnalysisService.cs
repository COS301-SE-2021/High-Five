using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisService
    {
        public Task<AnalyzedImageMetaData> AnalyzeImage(AnalyzeImageRequest request);
        public Task<AnalyzedVideoMetaData> AnalyzeVideo(AnalyzeVideoRequest request);
        public void SetBaseContainer(string containerName);
        public GetLiveAnalysisTokenResponse GetLiveAnalysisToken(string userId);
        public AnalysisResponse IsAnalyzeImageRequestValid(AnalyzeImageRequest request);
        public bool IsAnalyzeVideoRequestValid(AnalyzeVideoRequest request);
    }
}