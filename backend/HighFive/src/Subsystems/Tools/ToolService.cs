using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.Math;
using IronPython.Runtime.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Org.OpenAPITools.Models;
using src.Storage;

namespace src.Subsystems.Tools
{
    public class ToolService: IToolService
    {
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "tools";

        public ToolService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public async Task<bool> UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName)
        {
            var generatedToolName = _storageManager.HashMd5(toolName);
            if (ToolExists(generatedToolName))
            {
                return false;
            }
            
            var sourceCodeName = _storageManager.HashMd5(sourceCode.FileName);
            var sourceCodeFile = _storageManager.CreateNewFile(sourceCodeName + ".cs", ContainerName+ "/analysis/" + generatedToolName).Result;
            sourceCodeFile.AddMetadata("toolName",toolName);
            await sourceCodeFile.UploadFile(sourceCode);

            var modelNameArr = model.FileName.Split(".");
            var modelName = _storageManager.HashMd5(model.FileName);
            var modelFile = _storageManager
                .CreateNewFile(modelName + "." + modelNameArr[^1], ContainerName + "/analysis/" + generatedToolName).Result;
            await modelFile.UploadFile(model);

            AddToToolsFile(generatedToolName, "analysis");
            return true;
        }

        public async Task<bool> UploadDrawingTool(IFormFile sourceCode, string toolName)
        {
            var generatedToolName = _storageManager.HashMd5(toolName);
            if (ToolExists(generatedToolName))
            {
                return false;
            }
            
            var sourceCodeName = _storageManager.HashMd5(sourceCode.FileName);
            var sourceCodeFile = _storageManager.CreateNewFile(sourceCodeName + ".cs", ContainerName+ "/drawing/" + generatedToolName).Result;
            sourceCodeFile.AddMetadata("toolName",toolName);
            await sourceCodeFile.UploadFile(sourceCode);

            AddToToolsFile(generatedToolName, "drawing");
            return true;
        }

        public async Task<bool> DeleteTool(DeleteToolRequest request)
        {
            if (!RemoveFromToolsFile(request.ToolId, request.ToolType))
            {
                return false;
            }

            var toolFiles = _storageManager
                .GetAllFilesInContainer(ContainerName + "/" + request.ToolType + "/" + request.ToolId).Result;
            foreach (var file in toolFiles)
            {
                await file.Delete();
            }
            return true;
        }

        public List<Tool> GetAllTools()
        {
            var toolsList = new List<Tool>();
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsArray = toolsFile.ToText().Result.Split("\n");
            if (toolsArray[0].Equals(string.Empty))
            {
                return toolsList;
            }
            foreach (var tool in toolsArray)
            {
                var toolNameArr = tool.Split("/");
                var toolId = toolNameArr[^1];
                var newTool = new Tool
                {
                    ToolId = toolId,
                    ToolType = toolNameArr[0]
                };
                var toolFiles = _storageManager.GetAllFilesInContainer(ContainerName + "/" + tool).Result;
                foreach (var toolFile in toolFiles)
                {
                    var toolName = toolFile.GetMetaData("toolName");
                    if (toolName != null)
                    {
                        newTool.ToolName = toolName;
                        break;
                    }
                }
                toolsList.Add(newTool);
            }

            return toolsList;
        }

        public void StoreUserInfo(string id, string displayName, string email)
        {
            _storageManager.StoreUserInfo(id, displayName, email);
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
        
        private bool ToolExists(string toolName)
        {
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsArray = toolsFile.ToText().Result.Split("\n");
            return toolsArray.IndexOf("analysis/" + toolName) != -1 || toolsArray.IndexOf("drawing/" + toolName) != -1;
        }

        private void AddToToolsFile(string toolName, string type)
        {
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsText = toolsFile.ToText().Result;
            if (toolsText != string.Empty)
            {
                toolsText += "\n";
            }
            toolsText += type + "/" + toolName;

            toolsFile.UploadText(toolsText);
        }

        private bool RemoveFromToolsFile(string toolName, string type)
        {
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsList = toolsFile.ToText().Result.Split("\n");
            //the above line splits the text file's contents by newlines into an array
            var updatedToolsList = string.Empty;
            var removed = false;
            foreach (var tool in toolsList)
            {
                if (tool.Equals(type + "/" +toolName))
                {
                    removed = true;
                    continue;
                }
                updatedToolsList += tool;
                if (tool != toolsList[^1])
                {
                    updatedToolsList += "\n";
                }
            }

            toolsFile.UploadText(updatedToolsList);
            return removed;
        }
    }
}