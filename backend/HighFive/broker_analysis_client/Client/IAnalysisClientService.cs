using System.Threading.Tasks;
using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public interface IAnalysisClientService
    {
        public AnalyzedImageMetaData StoreImage(byte[] image, AnalyzeImageRequest request);
        public AnalyzedVideoMetaData StoreVideo(byte[] video, AnalyzeVideoRequest requests);
        public Task<byte[]> GetVideo(string videoId);
        public Task<byte[]> GetImage(string imageId);

        public string GetAnalysisTool(string toolId);
        public string GetDrawingTool(string toolId);
        public void UnloadAnalysisModel(string modelId);
        
        //TODO: GetPipeline functionality
    }
}