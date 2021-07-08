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
        /*
         *      Description:
         * This service class manages all the service contracts of the Pipelines subsystem. It is responsible
         * for creating, deleting and retrieving pipelines as well as adding or removing tools from pipelines
         * as well as 
         *
         *      Attributes:
         * -> _storageManager: a reference to the storage manager, used to access the blob storage.
         * -> _containerName: the name of the container in which a user's pipeline data is stored.
         */
        
        private readonly IStorageManager _storageManager;
        private string _containerName = "demo2pipelines";

        public PipelineService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public GetPipelinesResponse GetPipelines()
        {
            /*
             *      Description:
             * This function will return all the pipelines belonging to this user in the cloud storage.
             */
            
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

        public CreatePipelineResponse CreatePipeline(CreatePipelineRequest request)
        {
            /*
             *      Description:
             * This function will create and save a new analysis pipeline in the cloud storage.
             *
             *      Parameters:
             * -> request: contains the name of the pipeline as well as a list of tools that it should have
             *      initially.
             */
            
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

            var response = new CreatePipelineResponse()
            {
                PipelineId = generatedName
            };
            return response;
        }

        public bool AddTools(AddToolsRequest request)
        {
            /*
             *      Description:
             * This function will add a number of tools from a selected pipeline and udpate the new pipeline
             * in the cloud storage.
             *
             *      Parameters:
             * -> request: the request object for this service contract. It contains the pipeline id as well
             * as a list of tools to be added.
             */
            
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
            /*
             *      Description:
             * This function will remove a number of tools from a selected pipeline and update the new pipeline
             * in the cloud storage.
             *
             *      Parameters:
             * -> request: the request object for this function. It contains the id of the pipeline to be
             *      modified as well as a list of tools to be removed from the pipeline.
             */
            
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
            /*
             *      Description:
             * This function will delete a pipeline by a specific pipeline id.
             *
             *      Parameters:
             * -> request: the request object for this use case that contains the pipeline id to be deleted.
             */
            
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
            //the above line splits the text file's contents by newlines into an array
            return toolsArray;
        }

        private static Pipeline ConvertFileToPipeline(BlobFile file)
        {
            /*
             *      Description:
             * This is a helper function that converts a BlobFile object into a Pipeline object.
             *
             *      Parameters:
             * -> file: the file object that will be converted to a pipeline object.
             */
            
            var jsonData = file.ToText().Result;
            return JsonConvert.DeserializeObject<Pipeline>(jsonData);
        }

        private static void UploadPipelineToStorage(Pipeline pipeline, BlobFile blobFile)
        {
            /*
             *      Description:
             * This is a helper function that will upload a pipeline object to the cloud storage in a
             * provided blob file.
             *
             *      Parameters:
             * -> pipeline: the pipeline object to be uploaded to the cloud storage.
             * -> blobFile: the BlobFile object instantiated in an appropriate container which will be
             *      the reference to which the pipeline is uploaded.
             */
            
            var jsonData = JsonConvert.SerializeObject(pipeline);
            blobFile.UploadText(jsonData);
        }


        public void SetContainer(string container)
        {
            _containerName = container;
        }
    }
}