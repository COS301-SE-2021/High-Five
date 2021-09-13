using System.IO;
using System.Threading.Tasks;
using analysis_engine_v2.BrokerClient.Service.Models;
using broker_analysis_client.Client;
using broker_analysis_client.Client.Models;
using broker_analysis_client.Storage;
using Newtonsoft.Json;

namespace analysis_engine_v2.BrokerClient.Storage
{
    public class AnalysisStorageManager: IAnalysisStorageManager
    {
        private readonly IStorageManager _storageManager;

        public AnalysisStorageManager()
        {
            _storageManager = new StorageManager();
        }
        
        public async Task<AnalyzedImageMetaData> StoreImage(byte[] image, AnalyzeImageRequest request)
        {
            var analysisPipeline = JsonConvert.DeserializeObject<PipelineRequest>(GetPipeline(request.PipelineId).Result);
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/image";
            const string fileExtension = ".img";
            var analyzedMediaName = _storageManager.HashMd5(request.ImageId + "|" + string.Join(",",analysisPipeline.Tools));
            var testFile = _storageManager.CreateNewFile(analyzedMediaName+ fileExtension, storageContainer).Result;
            await testFile.UploadFileFromByteArray(image);

            var response = new AnalyzedImageMetaData
            {
                Id = analyzedMediaName,
                ImageId = request.ImageId,
                PipelineId = request.PipelineId,
                Url = testFile.GetUrl()
            };
            return response;
        }

        public async Task<AnalyzedVideoMetaData> StoreVideo(byte[] video, AnalyzeVideoRequest request)
        {
            var analysisPipeline = JsonConvert.DeserializeObject<PipelineRequest>(GetPipeline(request.PipelineId).Result);
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/video";
            const string fileExtension = ".mp4";
            var analyzedMediaName = _storageManager.HashMd5(request.VideoId + "|" + string.Join(",",analysisPipeline.Tools));
            var testFile = _storageManager.CreateNewFile(analyzedMediaName+ fileExtension, storageContainer).Result;
            await testFile.UploadFileFromByteArray(video);

            var response = new AnalyzedVideoMetaData
            {
                Id = analyzedMediaName,
                VideoId = request.VideoId,
                PipelineId = request.PipelineId,
                Url = testFile.GetUrl()
            };
            return response;
        }

        public string GetVideo(string videoId)
        {
            /*
             * Returns video as byte array
             */
            var video = _storageManager.GetFile(videoId + ".mp4", "video").Result;

            return video?.GetUrl();
        }

        public string GetImage(string imageId)
        {
            /*
             * Returns image as byte array
             */
            var image = _storageManager.GetFile(imageId + ".mp4", "image").Result;

            return image?.GetUrl();
        }

        public AnalysisToolComposite GetAnalysisTool(string toolId)
        {
            /*
             * Writes Model to disk and returns analysis source code
             */
            
            var toolSet = _storageManager.GetAllFilesInContainer("tools/analysis/" + toolId).Result;
            if (toolSet.Count == 0)
            {
                return null;
            }
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
            if (pipelineFile == null) return null;
            var pipelineObject = JsonConvert.DeserializeObject<PipelineRequest>(await pipelineFile.ToText());
            return FormatPipeline(pipelineObject);
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

        private string FormatPipeline(PipelineRequest request)
        {
            var toolIds = request.Tools;
            var resultStr = string.Empty;
            foreach (var toolId in toolIds)
            {
                resultStr += MapToolIdToString(toolId);
                if (toolId != toolIds[toolIds.Count - 1])
                {
                    resultStr += ",";
                }
            }
            return resultStr;
        }

        private string MapToolIdToString(string toolId)
        {
            return toolId switch
            {
                "D0" => "analysis:people",
                "D1" => "analysis:animal",
                "D2" => "analysis:vehicles",
                "D3" => "drawing:boxes",
                _ => "dynamic:" + toolId
            };
        }
    }
}