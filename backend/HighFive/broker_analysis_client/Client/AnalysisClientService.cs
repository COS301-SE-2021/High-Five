using System;
using System.IO;
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

        public AnalysisTool GetAnalysisTool(string toolId)
        {
            var toolSet = _storageManager.GetAllFilesInContainer("tools/analysis/" + toolId).Result;
            IBlobFile sourceCodeFile = null;
            IBlobFile modelFile = null;
            foreach (var tool in toolSet)
            {
                if (tool.GetMetaData("toolName") != null)
                {
                    sourceCodeFile = tool;
                }
                else
                {
                    modelFile = tool;
                }
            }

            var response = new AnalysisTool
            {
                ModelPath = Path.GetTempFileName()
            };

            var model = new FileStream(response.ModelPath, FileMode.Create);
            using var ms = modelFile.ToStream().Result;
            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, (int) ms.Length);
            model.Write(bytes, 0, bytes.Length);
            ms.Close();

            response.SourceCode = sourceCodeFile?.ToText().Result;
            
            return response;
        }

        public string GetDrawingTool(string toolId)
        {
            var toolSet = _storageManager.GetAllFilesInContainer("tools/drawing/" + toolId).Result;
            var drawingToolFile = toolSet[0];
            return drawingToolFile.ToText().Result;
        }

        public void UnloadAnalysisModel(string modelPath)
        {
            throw new System.NotImplementedException();
        }
    }
}