using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public interface IAnalysisClientService
    {
        public AnalyzedImageMetaData StoreImage(byte[] image);
        public AnalyzedVideoMetaData StoreVideo(byte[] video);
        public byte[] GetVideo(string videoId);
        public byte[] GetImage(string imageId);

        public string GetAnalysisTool(string toolId);
        public string GetDrawingTool(string toolId);
        public void UnloadAnalysisModel(string modelId);
        
        //TODO: GetPipeline functionality
    }
}