using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public interface IToolService
    {
        public Task<bool> UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName);
        public Task<bool> UploadDrawingTool(IFormFile sourceCode, string toolName);
        public Task<bool> DeleteTool(DeleteToolRequest request);
        public List<Tool> GetAllTools();
        public void StoreUserInfo(string id, string displayName, string email);
        public bool SetBaseContainer(string containerName);
        public List<string> GetToolTypes();
    }
}