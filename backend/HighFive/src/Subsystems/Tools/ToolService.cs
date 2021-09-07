﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Accord.Math;
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
            sourceCodeFile.AddMetadata("modelName", model.FileName);
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

        public List<string> GetToolTypes()
        {
            var responseList = new List<string> {"analysis", "drawing"};
            return responseList;
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
        
        //-----------------------------------TOOL LOADING FUNCTIONS-----------------------------------//
        //------------------------------Will be added to analysis engine------------------------------//

        private void LoadAnalysisTool(string id)
        {
            var toolFiles = _storageManager.GetAllFilesInContainer("tools/analysis/" + id).Result;
            if (toolFiles.Count == 0)
            {
                return;//invalid id
            }

            IBlobFile sourceCodeFile = null;
            IBlobFile modelFile = null;
            foreach (var toolFile in toolFiles)
            {
                if (toolFile.GetMetaData("modelName") != null)
                {
                    modelFile = toolFile;
                }
                else
                {
                    sourceCodeFile = toolFile;
                }
            }
            
            
            //write model to disk
            var modelStream = modelFile.ToStream().Result;
            var modelPath = Directory.GetCurrentDirectory() + "\\Models\\" + modelFile.GetMetaData("modelName");
            var fileStream = File.Create(modelPath);
            modelStream.Seek(0, SeekOrigin.Begin);
            modelStream.CopyTo(fileStream);
            fileStream.Close();
            
            //compile source code
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCodeFile.ToText().Result);
            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                /*
                 * TODO: in the references variable, all data types that are not in system must be
                 * explicitly referenced.
                 */
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };
            
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new []{syntaxTree},
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                //TODO: Handle errors here
                var failures = result.Diagnostics.Where(diagnostic => 
                    diagnostic.IsWarningAsError || 
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                {
                    Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                //TODO: This is where constructors/functions of compiled code is called
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                //parameter for assembly.GetType should be namespace.class
                var type = assembly.GetType("CustomTool.TestTool");
                //constructors can be called by passing parameters to Activator.CreateInstance
                var obj = Activator.CreateInstance(type);
                
                //First parameter in type.InvokeMember is function to be called. Last parameter is object of parameters
                var answer = type.InvokeMember("DoSomething",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    null
                );
            }
        }
    }
}