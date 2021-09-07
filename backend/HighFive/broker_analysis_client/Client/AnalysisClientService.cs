using broker_analysis_client.Client.Models;
using broker_analysis_client.Storage;

namespace broker_analysis_client.Client
{
    public class AnalysisClientService: IAnalysisClientService
    {
        private readonly IStorageManager _storageManager;

        public AnalysisClientService()
        {
            _storageManager = new StorageManager();
        }
        
        public AnalyzedImageMetaData StoreImage(byte[] image)
        {
            throw new System.NotImplementedException();
        }

        public AnalyzedVideoMetaData StoreVideo(byte[] video)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetVideo(string videoId)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetImage(string imageId)
        {
            throw new System.NotImplementedException();
        }

        public string GetAnalysisTool(string toolId)
        {
            throw new System.NotImplementedException();
        }

        public string GetDrawingTool(string toolId)
        {
            throw new System.NotImplementedException();
        }

        public void UnloadAnalysisModel(string modelId)
        {
            throw new System.NotImplementedException();
        }
    }
}