using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public interface IToolService
    {
        public Task<Tool> UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string metadataType, string toolName);
        public Task<Tool> UploadDrawingTool(IFormFile sourceCode, string metadataType, string toolName);
        public Task<bool> DeleteTool(DeleteToolRequest request);
        public List<Tool> GetAllTools();
        public void StoreUserInfo(string id, string displayName, string email);
        public bool SetBaseContainer(string containerName);
        public List<string> GetToolTypes();
        public Task<bool> CreateMetaDataType(IFormFile file, string name);
        public GetToolMetaDataTypes GetMetaDataTypes();
        public GetToolFilesResponse GetToolFiles(GetToolFilesRequest request);
    }
}