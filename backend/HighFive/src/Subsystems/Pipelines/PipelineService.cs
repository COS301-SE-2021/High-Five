using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.OpenAPITools.Models;
using src.Storage;
using static System.String;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace src.Subsystems.Pipelines
{
    public class PipelineService: IPipelineService
    {
        //NOTE: Does not check for duplicates in Tools
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "demo2pipelines";

        public PipelineService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public GetPipelinesResponse GetPipelines()
        {
            var allFiles = _storageManager.GetAllFilesInContainer(ContainerName);
            if (allFiles.Result == null)
            {
                return new GetPipelinesResponse{Pipelines = new List<Pipeline>()};
            }
            var resultList = new List<Pipeline>();
            foreach(var listBlobItem in allFiles.Result)
            {
                var currentPipeline = ConvertFileToPipeline(listBlobItem).Result;
                resultList.Add(currentPipeline);
            }
            var response = new GetPipelinesResponse {Pipelines = resultList};
            return response;
        }

        public void CreatePipeline(CreatePipelineRequest request)
        {
            var pipeline = request.Pipeline;
            var generatedName = _storageManager.HashMd5(pipeline.Name);
            var cloudBlockBlob = _storageManager.CreateNewFile(generatedName + ".json", ContainerName).Result;
            var salt = "";
            while (cloudBlockBlob == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(pipeline.Name+salt);
                cloudBlockBlob = _storageManager.CreateNewFile(generatedName + ".json", ContainerName).Result;
            }
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("originalName", pipeline.Name));
            if (!IsNullOrEmpty(salt))
            {
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("salt", salt));
            }
            var newPipeline = new Pipeline
            {
                Id = generatedName,
                Name = pipeline.Name,
                Tools = pipeline.Tools
            };
            UploadPipelineToStorage(newPipeline, cloudBlockBlob);
        }

        public bool AddTools(AddToolsRequest request)
        {
            var file =_storageManager.GetFile(request.PipelineId+".json", ContainerName).Result;
            if (file == null)
            {
                return false;
            }

            var pipeline = ConvertFileToPipeline(file).Result;
            var pipelineToolset = pipeline.Tools;
            pipelineToolset.AddRange(request.Tools);
            pipeline.Tools = pipelineToolset;
            UploadPipelineToStorage(pipeline, file);
            return true;
        }

        public void RemoveTools(RemoveToolsRequest request)
        {
            //NOTE: Currently does not check if anything is actually deleted
            var file =_storageManager.GetFile(request.PipelineId+".json", ContainerName).Result;
            if (file == null)
            {
                return;
            }

            var pipeline = ConvertFileToPipeline(file).Result;
            var pipelineToolset = pipeline.Tools;
            foreach (var tool in request.Tools)
            {
                pipelineToolset.Remove(tool);
            }
            pipeline.Tools = pipelineToolset;
            UploadPipelineToStorage(pipeline, file);
        }

        public async Task<bool> DeletePipeline(DeletePipelineRequest request)
        {
            CloudBlockBlob file = _storageManager.GetFile(request.PipelineId + ".json", ContainerName).Result;
            if (file == null)
            {
                return false;
            }
            await file.DeleteIfExistsAsync();
            return true;
        }

        private async Task<Pipeline> ConvertFileToPipeline(CloudBlockBlob file)
        {
            var jsonData = await file.DownloadTextAsync();
            return JsonConvert.DeserializeObject<Pipeline>(jsonData);
        }

        private void UploadPipelineToStorage(Pipeline pipeline, CloudBlockBlob cloudBlockBlob)
        {
            var jsonData = JsonConvert.SerializeObject(pipeline);
            cloudBlockBlob.UploadTextAsync(jsonData);
        }
    }
}