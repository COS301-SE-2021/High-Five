using System.Threading.Tasks;
using Accord.Math;
using Microsoft.AspNetCore.Http;
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

            return true;
        }

        public void UploadDrawingTool(IFormFile sourceCode, string toolName)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTool(DeleteToolRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void GetAllTools()
        {
            throw new System.NotImplementedException();
        }

        private bool ToolExists(string toolName)
        {
            var toolsFile = _storageManager.GetFile("tools.txt", "").Result;
            var toolsArray = toolsFile.ToText().Result.Split("\n");
            return toolsArray.IndexOf(toolName) != -1;
        }
    }
}