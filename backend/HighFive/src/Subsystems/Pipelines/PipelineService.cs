using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using src.Storage;
using static System.String;

namespace src.Subsystems.Pipelines
{
    public class PipelineService: IPipelineService
    {
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "demo2pipelines";

        public PipelineService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public GetPipelinesResponse GetPipelines()
        {
            throw new System.NotImplementedException();
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
            var jsonData = JsonConvert.SerializeObject(newPipeline);
            cloudBlockBlob.Properties.ContentType = "application/json";
            cloudBlockBlob.UploadTextAsync(jsonData);
        }

        public void AddTools(AddToolsRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTools(RemoveToolsRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePipeline(DeletePipelineRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}