using System.Threading.Tasks;
using broker_analysis_client.Client.Models;

namespace analysis_engine_v2.BrokerClient.Storage
{
    public interface IAnalysisStorageManager
    {
        public Task<AnalyzedImageMetaData> StoreImage(byte[] image, AnalyzeImageRequest request);
        public Task<AnalyzedVideoMetaData> StoreVideo(byte[] video, AnalyzeVideoRequest requests);
        public string GetVideo(string videoId);
        public string GetImage(string imageId);

        public AnalysisToolComposite GetAnalysisTool(string toolId);
        public string GetDrawingTool(string toolId);
        public void UnloadAnalysisModel(string modelPath);

        public Task<string> GetPipeline(string pipelineId);
        public Task<string> GetMetadataType(string metadataTypeName);
        public Task<string> GetLivePipeline();
    }
}