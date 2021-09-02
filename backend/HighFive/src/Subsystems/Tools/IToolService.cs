using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public interface IToolService
    {
        public Task<bool> UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName);
        public void UploadDrawingTool(IFormFile sourceCode, string toolName);
        public void DeleteTool(DeleteToolRequest request);
        public void GetAllTools();
        public void StoreUserInfo(string id, string displayName, string email);
        public bool SetBaseContainer(string containerName);
    }
}