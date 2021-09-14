using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.Tools;
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
        private const string ContainerName = "pipeline";
        private readonly IToolService _toolService;

        public PipelineService(IStorageManager storageManager, IToolService toolService)
        {
            _storageManager = storageManager;
            _toolService = toolService;
        }

        public GetPipelinesResponse GetPipelines()
        {
            /*
             *      Description:
             * This function will return all the pipelines belonging to this user in the cloud storage.
             */

            var allFiles = _storageManager.GetAllFilesInContainer(ContainerName);
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

        public async Task<CreatePipelineResponse> CreatePipeline(CreatePipelineRequest request)
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
            var blobFile = _storageManager.CreateNewFile(generatedName + ".json", ContainerName).Result;
            var salt = "";
            while (blobFile == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(pipeline.Name+salt);
                blobFile = _storageManager.CreateNewFile(generatedName + ".json", ContainerName).Result;
            }

            blobFile.AddMetadata("originalName", pipeline.Name);
            if (!IsNullOrEmpty(salt))
            {
                blobFile.AddMetadata("salt", salt);
            }

            var temp = new List<string>(pipeline.Tools);
            for (var k = 0; k < pipeline.Tools.Count; k++)
            {
                pipeline.Tools[k] = ToolNameToId(pipeline.Tools[k]);
            }
            var newPipeline = new Pipeline
            {
                Id = generatedName,
                Name = pipeline.Name,
                Tools = pipeline.Tools,
                MetadataType = pipeline.MetadataType
            };
            await UploadPipelineToStorage(newPipeline, blobFile);
            newPipeline.Tools = temp;
            var response = new CreatePipelineResponse()
            {
                Pipeline = newPipeline
            };
            return response;
        }

        public async Task<bool> AddTools(AddToolsRequest request)
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

            var file =_storageManager.GetFile(request.PipelineId+".json", ContainerName).Result;
            if (file == null)
            {
                return false;
            }

            var pipeline = ConvertFileToPipeline(file);
            for (var k = 0; k < pipeline.Tools.Count; k++)
            {
                pipeline.Tools[k] = ToolNameToId(pipeline.Tools[k]);
            }
            var pipelineToolset = pipeline.Tools;
            for (var k = 0; k < request.Tools.Count; k++)
            {
                request.Tools[k] = ToolNameToId(request.Tools[k]);
            }
            pipelineToolset.AddRange(request.Tools);
            pipeline.Tools = pipelineToolset.Distinct().ToList();
            await UploadPipelineToStorage(pipeline, file);
            return true;
        }

        public async Task<bool> RemoveTools(RemoveToolsRequest request)
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

            var file =_storageManager.GetFile(request.PipelineId+".json", ContainerName).Result;
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

            for (var k = 0; k < pipelineToolset.Count; k++)
            {
                pipelineToolset[k] = ToolNameToId(pipelineToolset[k]);
            }
            pipeline.Tools = pipelineToolset;
            await UploadPipelineToStorage(pipeline, file);
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

            var blobFile = _storageManager.GetFile(request.PipelineId + ".json", ContainerName).Result;
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
             * cloud storage under a public container.
             */

            var oldContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            _storageManager.SetBaseContainer(oldContainer);
            var toolsArray = toolsFile.ToText().Result.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            //the above line splits the text file's contents by newlines into an array
            return toolsArray;
        }

        private Pipeline ConvertFileToPipeline(IBlobFile file)
        {
            /*
             *      Description:
             * This is a helper function that converts a BlobFile object into a Pipeline object. All tool
             * id's in the pipeline are converted to their tool names before the pipeline is returned.
             *
             *      Parameters:
             * -> file: the file object that will be converted to a pipeline object.
             */

            var jsonData = file.ToText().Result;
            var pipeline = JsonConvert.DeserializeObject<Pipeline>(jsonData);
            var toolIdCopy = new List<string>(pipeline.Tools);
            for (var k = 0; k < pipeline.Tools.Count; k++)
            {
                var toolId = pipeline.Tools[k];
                var toolName = ToolIdToName(toolId);
                if (toolName == null)
                {
                    toolIdCopy.Remove(toolId);
                }
                else
                {
                    pipeline.Tools[k] = toolName;
                }
            }

            var temp = new List<string>(pipeline.Tools);
            pipeline.Tools = toolIdCopy;
            file.UploadText(JsonConvert.SerializeObject(pipeline));//removes unused tools
            pipeline.Tools = temp;
            return pipeline;
        }

        private static async Task UploadPipelineToStorage(Pipeline pipeline, IBlobFile blobFile)
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
            await blobFile.UploadText(jsonData);
        }

        public bool SetBaseContainer(string containerName)
        {
            /*
             *      Description:
             * This function tests if a baseContainer has been set yet, it will be called before any of the
             * other StorageManager method code executes. If a base container has already been set, this code
             * will do nothing, else it will set the base container to the user's Azure AD B2C unique object
             * id - hence pointing towards the user's own container within the storage.
             *
             *      Parameters:
             * -> containerName: the user's id that will be used as the container name.
             */

            if (!_storageManager.IsContainerSet())
            {
                return _storageManager.SetBaseContainer(containerName).Result;
            }

            return true;
        }

        public GetPipelineIdsResponse GetPipelineIds()
        {
            /*
             *      Description:
             * This function will return the unique id of every pipeline belonging to this user.
             */

            var allFiles = _storageManager.GetAllFilesInContainer(ContainerName);
            if (allFiles.Result == null)
            {
                return new GetPipelineIdsResponse{PipelineIds = new List<string>()};
            }
            var idList = new List<string>();
            foreach(var listBlobItem in allFiles.Result)
            {
                var currentPipeline = ConvertFileToPipeline(listBlobItem);
                idList.Add(currentPipeline.Id);
            }
            var response = new GetPipelineIdsResponse {PipelineIds = idList};
            return response;
        }

        public async Task<Pipeline> GetPipeline(GetPipelineRequest request)
        {
            /*
             *      Description:
             * This function will return a pipeline based off a provided pipeline id.
             *
             *      Parameters:
             * -> request: the request body containing the pipeline Id for the service contract.
             */
            var pipelineFile = _storageManager.GetFile(request.PipelineId + ".json", ContainerName).Result;
            if (pipelineFile == null || !await pipelineFile.Exists()) return null;
            var pipeline = ConvertFileToPipeline(pipelineFile);
            return pipeline;
        }

        public void StoreUserInfo(string id, string displayName, string email)
        {
            _storageManager.StoreUserInfo(id, displayName, email);
        }

        public async Task<bool> SetLivePipeline(GetPipelineRequest request)
        {
            var livePipelineFile = _storageManager.GetFile("live_pipeline.txt", "").Result;
            if (livePipelineFile == null)
            {
                livePipelineFile = _storageManager.CreateNewFile("live_pipeline.txt", "").Result;
            }

            await livePipelineFile.UploadText(request.PipelineId);
            return true;
        }

        public async Task<Pipeline> GetLivePipeline()
        {
            var livePipelineFile = _storageManager.GetFile("live_pipeline.txt", "").Result;
            if (livePipelineFile == null)
            {
                return null;
            }

            var request = new GetPipelineRequest {PipelineId = await livePipelineFile.ToText()};
            var livePipeline = GetPipeline(request).Result;
            return livePipeline;
        }

        private string ToolNameToId(string toolName)
        {
            return toolName switch
            {
                "PeopleRecognition" => "D0",
                "AnimalRecognition" => "D1",
                "VehicleRecognition" => "D2",
                "FastVehicleRecognition" => "D3",
                "BoxDrawingTool" => "D4",
                _ => FindToolByName(toolName)
            };
        }

        private string ToolIdToName(string toolId)
        {
            return FindToolById(toolId);
        }

        private string FindToolByName(string toolName)
        {
            return FindToolById(_storageManager.HashMd5(toolName));
        }

        private string FindToolById(string toolId)
        {
            var allTools = _toolService.GetAllTools();
            foreach (var tool in allTools)
            {
                if (tool.ToolId.Equals(toolId))
                {
                    return tool.ToolName;
                }
            }
            return null; //tool does not exist
        }

    }
}
