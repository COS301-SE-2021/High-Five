using System;
using System.IO;
using System.Threading.Tasks;
using broker_analysis_client.Client.Models;
using broker_analysis_client.Storage;

namespace broker_analysis_client.Client
{
    public class AnalysisStorageManager: IAnalysisStorageManager
    {
        private readonly IStorageManager _storageManager;

        public AnalysisStorageManager()
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
            /*
             * Returns video as byte array
             */
            var video = _storageManager.GetFile(videoId + ".mp4", "video").Result;
            if (video == null)
            {
                return null;
            }

            return await video.ToByteArray();
        }

        public async Task<byte[]> GetImage(string imageId)
        {
            /*
             * Returns image as byte array
             */
            var image = _storageManager.GetFile(imageId + ".mp4", "image").Result;
            if (image == null)
            {
                return null;
            }

            return await image.ToByteArray();
        }

        public AnalysisToolComposite GetAnalysisTool(string toolId)
        {
            /*
             * Writes Model to disk and returns analysis source code
             */
            
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

            var response = new AnalysisToolComposite
            {
                ModelPath = Path.GetTempFileName()
            };

            var model = File.Create(response.ModelPath);
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
            /*
             * Returns source code for Drawing Tool
             */
            var toolSet = _storageManager.GetAllFilesInContainer("tools/drawing/" + toolId).Result;
            var drawingToolFile = toolSet[0];
            return drawingToolFile.ToText().Result;
        }

        public void UnloadAnalysisModel(string modelPath)
        {
            /*
             * Deletes Analysis Model that was written to disk during GetAnalysisTool
             */
            File.Delete(modelPath);
        }

        public async Task<string> GetPipeline(string pipelineId)
        {
            /*
             * Returns pipeline as JSON
             */
            var pipelineFile = _storageManager.GetFile(pipelineId + ".json", "pipeline").Result;
            if (pipelineFile == null || !await pipelineFile.Exists()) return null;
            return await pipelineFile.ToText();
        }

        public async Task<string> GetMetadataType(string metadataTypeName)
        {
            /*
             * Returns source code for custom metadata type
             */
            var metadataFile =
                _storageManager.GetFile(_storageManager.HashMd5(metadataTypeName) + ".cs", "tool/metadata").Result;
            if (metadataFile == null)
            {
                return null;
            }

            return await metadataFile.ToText();
        }
    }
}