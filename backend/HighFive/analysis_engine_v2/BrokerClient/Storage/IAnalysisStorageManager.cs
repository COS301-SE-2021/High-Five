using System.IO;
using System.Threading.Tasks;
using broker_analysis_client.Client.Models;

namespace analysis_engine_v2.BrokerClient.Storage
{
    public interface IAnalysisStorageManager
    {
        public Task<AnalyzedImageMetaData> StoreImage(string imagePath, AnalyzeImageRequest request);
        public Task<AnalyzedVideoMetaData> StoreVideo(string videoPath, AnalyzeVideoRequest requests);
        public string GetVideo(string videoId);
        public Stream GetImage(string imageId);

        public Task<AnalysisToolComposite> GetAnalysisTool(string toolId);
        //public byte[] GetDrawingTool(string toolId);
        public byte[] GetDrawingTool(string toolId);
        public void UnloadAnalysisModel(string modelPath);

        public Task<string> GetPipeline(string pipelineId, bool format=true);
        public Task<string> GetMetadataType(string metadataTypeName);
        public Task<string> GetLivePipeline();
    }
}