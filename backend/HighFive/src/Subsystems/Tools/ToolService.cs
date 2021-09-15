﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Accord.Math;
using IronPython.Modules;
using IronPython.Runtime.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
        
        public async Task<Tool> UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string metadataType, string toolName)
        {
            if (sourceCode == null || model == null)
            {
                return null;
            }
            var generatedToolName = _storageManager.HashMd5(toolName);
            var sourceCodeName = _storageManager.HashMd5(sourceCode.FileName);
            var sourceCodeFile = _storageManager.GetFile(sourceCodeName + ".dll",ContainerName+ "/analysis/" + generatedToolName).Result;
            if (sourceCodeFile != null)
            {
                return null;
            }
            
            sourceCodeFile = _storageManager.CreateNewFile(sourceCodeName + ".dll", ContainerName+ "/analysis/" + generatedToolName).Result;
            sourceCodeFile.AddMetadata("toolName",toolName);
            sourceCodeFile.AddMetadata("metadataType", metadataType);
            await sourceCodeFile.UploadFile(sourceCode);
            if (!validateDll(sourceCodeFile))
            {
                await sourceCodeFile.Delete();
                throw new InvalidDataException();
            }

            var modelNameArr = model.FileName.Split(".");
            var modelName = _storageManager.HashMd5(model.FileName);
            sourceCodeFile.AddMetadata("modelName", model.FileName);
            var modelFile = _storageManager
                .CreateNewFile(modelName + "." + modelNameArr[^1], ContainerName + "/analysis/" + generatedToolName).Result;
            if (modelFile != null)
            {
                modelFile.AddMetadata("modelName", model.FileName);
                await modelFile?.UploadFile(model);
            }

            AddToToolsFile(generatedToolName, "analysis", metadataType);
            return new Tool
            {
                ToolId = generatedToolName,
                ToolName = toolName,
                ToolType = "analysis",
                ToolMetadataType = metadataType
            };
        }

        public async Task<Tool> UploadDrawingTool(IFormFile sourceCode, string metadataType, string toolName)
        {
            if (sourceCode == null)
            {
                return null;
            }
            var generatedToolName = _storageManager.HashMd5(toolName);
            var sourceCodeName = _storageManager.HashMd5(sourceCode.FileName);
            var sourceCodeFile = _storageManager.GetFile(sourceCodeName + ".dll",ContainerName+ "/drawing/" + generatedToolName).Result;
            if (sourceCodeFile != null)
            {
                return null;
            }
            sourceCodeFile = _storageManager.CreateNewFile(sourceCodeName + ".dll", ContainerName+ "/drawing/" + generatedToolName).Result;
            sourceCodeFile.AddMetadata("toolName",toolName);
            sourceCodeFile.AddMetadata("metadataType", metadataType);
            await sourceCodeFile.UploadFile(sourceCode);
            if (!validateDll(sourceCodeFile))
            {
                await sourceCodeFile.Delete();
                throw new InvalidDataException();
            }

            AddToToolsFile(generatedToolName, "drawing", metadataType);
            return new Tool
            {
                ToolId = generatedToolName,
                ToolName = toolName,
                ToolType = "analysis",
                ToolMetadataType = metadataType
            };
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
            var defaultCounter = 0;
            foreach(var defaultToolString in GetDefaultTools())
            {
                var toolNameArr = defaultToolString.Split("/");
                var newTool = new Tool
                {
                    ToolId = "D" + defaultCounter++,
                    ToolName = toolNameArr[1],
                    ToolType = toolNameArr[0],
                    ToolMetadataType = toolNameArr[2],
                    IsDefaultTool = true
                };
                toolsList.Add(newTool);
            }
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsArray = toolsFile.ToText().Result.Split("\n");
            if (toolsArray[0].Equals(string.Empty))
            {
                return toolsList;
            }
            foreach (var tool in toolsArray)
            {
                var toolNameArr = tool.Split("/");
                var newTool = new Tool
                {
                    ToolId = toolNameArr[1],
                    ToolType = toolNameArr[0],
                    ToolMetadataType = toolNameArr[2],
                    IsDefaultTool = false
                };
                var toolDirectory = toolNameArr[0] + "/" + toolNameArr[1];
                var toolFiles = _storageManager.GetAllFilesInContainer(ContainerName + "/" + toolDirectory).Result;
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

        public List<string> GetToolTypes()
        {
            var responseList = new List<string> {"analysis", "drawing"};
            return responseList;
        }

        public async Task<bool> CreateMetaDataType(IFormFile file, string name)
        {
            var generatedName = _storageManager.HashMd5(file.Name);
            var metadataFile = _storageManager.CreateNewFile(generatedName + ".cs", ContainerName + "/metadata").Result;
            if (metadataFile == null || GetDefaultToolMetaData().IndexOf(name) != -1)
            {
                return false;
            }
            
            metadataFile.AddMetadata("name", name);
            await metadataFile.UploadFile(file);

            var metadataListFile = _storageManager.GetFile("toolmetadata.txt", "").Result;
            var metadataText = metadataListFile.ToText().Result;
            if (metadataText != string.Empty)
            {
                metadataText += "\n";
            }
            metadataText +=  name;

            await metadataListFile.UploadText(metadataText);
            
            return true;
        }

        public GetToolMetaDataTypes GetMetaDataTypes()
        {
            var responseList = new List<string>();
            var metadataFile = _storageManager.GetFile("toolmetadata.txt", "").Result;
            var metadataArray = metadataFile.ToText().Result.Split(new[] {'\n'}, StringSplitOptions.None);
            //the above line splits the text file's contents by newlines into an array
            var defaultMetadata = GetDefaultToolMetaData();
            responseList.AddRange(metadataArray);
            responseList.AddRange(defaultMetadata);

            return new GetToolMetaDataTypes{MetaDataTypes = responseList};
        }

        public GetToolFilesResponse GetToolFiles(GetToolFilesRequest request)
        {
            if(!request.ToolType.Equals("drawing") && !request.ToolType.Equals("analysis"))
            {
                throw new FormatException();
            }
            var toolSet = _storageManager.GetAllFilesInContainer(ContainerName + "/" + request.ToolType + "/" + request.ToolId).Result;
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

            var response = new GetToolFilesResponse
            {
                ToolSourceCode = new FileDownload {FileUrl = sourceCodeFile?.GetUrl()},
                Model = new FileDownload {FileUrl = modelFile?.GetUrl()}
            };
            return response;
        }

        private void AddToToolsFile(string toolName, string type, string metadataType)
        {
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsText = toolsFile.ToText().Result;
            if (toolsText != string.Empty)
            {
                toolsText += "\n";
            }
            toolsText += type + "/" + toolName + "/" + metadataType;

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
                if (tool.Contains(type + "/" +toolName))
                {
                    removed = true;
                    if (tool == toolsList[^1])
                    {
                        updatedToolsList = updatedToolsList.TrimEnd('\n');
                    }
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

        private string[] GetDefaultTools()
        {
            var currentContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var defaultToolsFile = _storageManager.GetFile("default_tools.txt","").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var toolsArray = defaultToolsFile.ToText().Result.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            //the above line splits the text file's contents by newlines into an array
            return toolsArray;
        }
        
        private string[] GetDefaultToolMetaData()
        {
            var currentContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var defaultMetadataFile = _storageManager.GetFile("default_toolmetadata.txt","").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var metadataArray = defaultMetadataFile.ToText().Result.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            //the above line splits the text file's contents by newlines into an array
            return metadataArray;
        }

        private bool validateDll(IBlobFile file)
        {
            try
            {
                var dllBytes = file.ToByteArray().Result;
                var assembly = Assembly.Load(dllBytes);
                var dynamicType = assembly.GetType("High5.CustomTool");
                var obj = Activator.CreateInstance(dynamicType);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}