using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public interface IAnalysisClientService
    {
        public AnalyzedImageMetaData AnalyzeImage(AnalyzeImageRequest request);
        public AnalyzedVideoMetaData AnalyzeVideo(AnalyzeVideoRequest request);
    }
}