using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
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
        private string _containerName = "demo2pipelines";

        public PipelineService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public GetPipelinesResponse GetPipelines()
        {
            var allFiles = _storageManager.GetAllFilesInContainer(_containerName);
            if (allFiles.Result == null)
            {
                return new GetPipelinesResponse{Pipelines = new List<Pipeline>()};
            }
            var resultList = new List<Pipeline>();
            foreach(var listBlobItem in allFiles.Result)
            {
                var currentPipeline = ConvertFileToPipeline(listBlobItem);
                resultList.Add(currentPipeline);
            }
            var response = new GetPipelinesResponse {Pipelines = resultList};
            return response;
        }

        public void CreatePipeline(CreatePipelineRequest request)
        {
            var pipeline = request.Pipeline;
            var generatedName = _storageManager.HashMd5(pipeline.Name);
            var blobFile = _storageManager.CreateNewFile(generatedName + ".json", _containerName).Result;
            var salt = "";
            while (blobFile == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(pipeline.Name+salt);
                blobFile = _storageManager.CreateNewFile(generatedName + ".json", _containerName).Result;
            }

            blobFile.AddMetadata("originalName", pipeline.Name);
            if (!IsNullOrEmpty(salt))
            {
                blobFile.AddMetadata("salt", salt);
            }
            var newPipeline = new Pipeline
            {
                Id = generatedName,
                Name = pipeline.Name,
                Tools = pipeline.Tools
            };
            UploadPipelineToStorage(newPipeline, blobFile);
        }

        public bool AddTools(AddToolsRequest request)
        {
            var file =_storageManager.GetFile(request.PipelineId+".json", _containerName).Result;
            if (file == null)
            {
                return false;
            }

            var pipeline = ConvertFileToPipeline(file);
            var pipelineToolset = pipeline.Tools;
            pipelineToolset.AddRange(request.Tools);
            pipeline.Tools = pipelineToolset.Distinct().ToList();
            UploadPipelineToStorage(pipeline, file);
            return true;
        }

        public bool RemoveTools(RemoveToolsRequest request)
        {
            var file =_storageManager.GetFile(request.PipelineId+".json", _containerName).Result;
            if (file == null)
            {
                return false;
            }

            var pipeline = ConvertFileToPipeline(file);
            var pipelineToolset = pipeline.Tools;
            foreach (var tool in request.Tools)
            {
                pipelineToolset.Remove(tool);
            }
            pipeline.Tools = pipelineToolset;
            UploadPipelineToStorage(pipeline, file);
            return true;
        }

        public async Task<bool> DeletePipeline(DeletePipelineRequest request)
        {
            var blobFile = _storageManager.GetFile(request.PipelineId + ".json", _containerName).Result;
            if (blobFile == null)
            {
                return false;
            }
            await blobFile.Delete();
            return true;
        }

        public string[] GetAllTools()
        {
            /*
             *      Description:
             * This function will return all the existing tools as stored in a singular text file in the
             * cloud storage.
             */

            var oldContainer = _containerName;
            _containerName = "public";
            var toolsFile = _storageManager.GetFile("tools.txt", _containerName).Result;
            _containerName = oldContainer;
            var toolsArray = toolsFile.ToText().Result.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            //the above line splits the textfile's contents by newlines into an array
            return toolsArray;
        }

        private static Pipeline ConvertFileToPipeline(BlobFile file)
        {
            var jsonData = file.ToText().Result;
            return JsonConvert.DeserializeObject<Pipeline>(jsonData);
        }

        private static void UploadPipelineToStorage(Pipeline pipeline, BlobFile blobFile)
        {
            var jsonData = JsonConvert.SerializeObject(pipeline);
            blobFile.UploadText(jsonData);
        }


        public void SetContainer(string container)
        {
            _containerName = container;
        }
    }
}