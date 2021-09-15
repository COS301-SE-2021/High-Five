using System;
using System.IO;
using System.Threading.Tasks;
using analysis_engine_v2.BrokerClient.Service.Models;
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
            _storageManager = StorageManagerContainer.StorageManager;
        }
        
        public async Task<AnalyzedImageMetaData> StoreImage(string imagePath, AnalyzeImageRequest request)
        {
            var image = File.ReadAllBytes(imagePath);
            var analysisPipeline = JsonConvert.DeserializeObject<PipelineRequest>(GetPipeline(request.PipelineId, false).Result);
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/image";
            const string fileExtension = ".img";
            var analyzedMediaName = _storageManager.HashMd5(request.ImageId + "|" + analysisPipeline.Id);
            var testFile = _storageManager.CreateNewFile(analyzedMediaName+ fileExtension, storageContainer).Result;
            testFile.AddMetadata("imageId", request.ImageId);
            testFile.AddMetadata("pipelineId", request.PipelineId);
            await testFile.UploadFileFromByteArray(image, "image/jpg");

            var response = new AnalyzedImageMetaData
            {
                Id = analyzedMediaName,
                ImageId = request.ImageId,
                PipelineId = request.PipelineId,
                Url = testFile.GetUrl(),
                DateAnalyzed = testFile.Properties.LastModified.Value.DateTime
            };
            return response;
        }

        public async Task<AnalyzedVideoMetaData> StoreVideo(string videoPath, AnalyzeVideoRequest request)
        {
            var analysisPipeline = JsonConvert.DeserializeObject<PipelineRequest>(GetPipeline(request.PipelineId, false).Result);
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/video";
            const string fileExtension = ".mp4";
            var analyzedMediaName = _storageManager.HashMd5(request.VideoId + "|" + request.PipelineId);
            
            /*var thumbnailPath = Path.GetTempFileName();
            await _videoDecoder.GetThumbnailFromVideo(videoPath, thumbnailPath);*/
            var originalThumbnailFile = _storageManager.GetFile(request.VideoId + "-thumbnail.jpg", "video").Result;
            var thumbnailFile = _storageManager.CreateNewFile(analyzedMediaName + "-thumbnail.jpg", storageContainer).Result;
            await thumbnailFile.UploadFileFromStream(await originalThumbnailFile.ToStream(), "image/jpg");
            
            var testFile = _storageManager.CreateNewFile(analyzedMediaName+ fileExtension, storageContainer).Result;
            testFile.AddMetadata("videoId", request.VideoId);
            testFile.AddMetadata("pipelineId", request.PipelineId);
            await testFile.UploadFile(videoPath, "video/mp4");

            var response = new AnalyzedVideoMetaData
            {
                Id = analyzedMediaName,
                VideoId = request.VideoId,
                PipelineId = request.PipelineId,
                Url = testFile.GetUrl(),
                DateAnalyzed = testFile.Properties.LastModified.Value.DateTime,
                Thumbnail = thumbnailFile.GetUrl()
            };
            return response;
        }

        public string GetVideo(string videoId)
        {
            /*
             * Returns video as url
             */
            var video = _storageManager.GetFile(videoId + ".mp4", "video").Result;

            return video?.GetUrl();
        }

        public Stream GetImage(string imageId)
        {
            /*
             * Returns image as url
             */
            var image = _storageManager.GetFile(imageId + ".img", "image").Result;

            return image?.ToStream().Result;
        }

        public AnalysisToolComposite GetAnalysisTool(string toolId)
        {
            /*
             * Writes Model to disk and returns analysis source code
             */
            Console.WriteLine("Searching in " + "tools/analysis/" + toolId);
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
                ModelPath = Directory.GetCurrentDirectory() + @"\..\..\Models\"
            };

            var model = File.Create(response.ModelPath + modelFile.GetMetaData("modelName"));
            using var ms = modelFile.ToStream().Result;
            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, (int) ms.Length);
            model.Write(bytes, 0, bytes.Length);
            ms.Close();

            response.ByteData = sourceCodeFile?.ToByteArray().Result;
            
            return response;
        }

        public byte[] GetDrawingTool(string toolId)
        {
            /*
             * Returns source code for Drawing Tool
             */
            var toolSet = _storageManager.GetAllFilesInContainer("tools/drawing/" + toolId).Result;
            var drawingToolFile = toolSet[0];
            return drawingToolFile.ToByteArray().Result;
        }

        public void UnloadAnalysisModel(string modelPath)
        {
            /*
             * Deletes Analysis Model that was written to disk during GetAnalysisTool
             */
            File.Delete(modelPath);
        }

        public async Task<string> GetPipeline(string pipelineId, bool format = true)
        {
            /*
             * Returns pipeline as JSON
             */
            var pipelineFile = _storageManager.GetFile(pipelineId + ".json", "pipeline").Result;
            if (pipelineFile == null) return null;
            var pipelineObject = JsonConvert.DeserializeObject<PipelineRequest>(await pipelineFile.ToText());
            if (format)
            {
                return FormatPipeline(pipelineObject);
            }
            else
            {
                return JsonConvert.SerializeObject(pipelineObject);
            }
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
                "D3" => "analysis:fastvehicles",
                "D4" => "drawing:boxes",
                _ => "dynamic:" + _storageManager.HashMd5(toolId)
            };
        }
        
        public async Task<string> GetLivePipeline()
        {
            var livePipeline = _storageManager.GetFile("default_pipeline.txt", "").Result;
            if (livePipeline == null)
            {
                return null;
            }
        
            var pipelineObject = JsonConvert.DeserializeObject<PipelineRequest>(await livePipeline.ToText());
            return FormatPipeline(pipelineObject);
        }

        private string GetVideoThumbnail(string videoId)
        {
            var file = _storageManager.GetFile(videoId, "video").Result;
            return file.GetUrl();
        }

    }
}