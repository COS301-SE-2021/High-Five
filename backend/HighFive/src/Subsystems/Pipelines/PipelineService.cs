using System.Collections.Generic;
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
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "demo2pipelines";

        public PipelineService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public async Task<GetPipelinesResponse> GetPipelines()
        {
            var allFiles = _storageManager.GetAllFilesInContainer(ContainerName);
            if (allFiles.Result == null)
            {
                return new GetPipelinesResponse{Pipelines = new List<Pipeline>()};
            }
            var resultList = new List<Pipeline>();
            foreach(var listBlobItem in allFiles.Result)
            {
                var jsonData = await listBlobItem.DownloadTextAsync();
                var currentPipeline = JsonConvert.DeserializeObject<Pipeline>(jsonData);
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