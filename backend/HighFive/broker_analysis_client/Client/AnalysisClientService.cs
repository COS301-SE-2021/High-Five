using System;
using System.Threading.Tasks;
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
        
        public AnalyzedImageMetaData StoreImage(byte[] image, AnalyzeImageRequest request)
        {
            //var blobFile = _storageManager.CreateNewFile(fileName, "analyzed/video").Result;
            throw new NotImplementedException();
        }

        public AnalyzedVideoMetaData StoreVideo(byte[] video, AnalyzeVideoRequest requests)
        {
            throw new System.NotImplementedException();
        }

        public async Task<byte[]> GetVideo(string videoId)
        {
            var video = _storageManager.GetFile(videoId + ".mp4", "video").Result;
            if (!await video.Exists())
            {
                return null;
            }

            return await video.ToByteArray();
        }

        public async Task<byte[]> GetImage(string imageId)
        {
            var image = _storageManager.GetFile(imageId + ".mp4", "image").Result;
            if (!await image.Exists())
            {
                return null;
            }

            return await image.ToByteArray();
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