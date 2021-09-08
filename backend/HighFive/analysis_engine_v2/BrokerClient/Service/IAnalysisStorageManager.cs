using System.Threading.Tasks;
using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public interface IAnalysisStorageManager
    {
        public Task<AnalyzedImageMetaData> StoreImage(byte[] image, AnalyzeImageRequest request);
        public AnalyzedVideoMetaData StoreVideo(byte[] video, AnalyzeVideoRequest requests);
        public Task<byte[]> GetVideo(string videoId);
        public Task<byte[]> GetImage(string imageId);

        public AnalysisToolComposite GetAnalysisTool(string toolId);
        public string GetDrawingTool(string toolId);
        public void UnloadAnalysisModel(string modelPath);

        public Task<string> GetPipeline(string pipelineId);
        public Task<string> GetMetadataType(string metadataTypeName);
    }
}