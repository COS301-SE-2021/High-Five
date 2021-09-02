using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public class ToolService: IToolService
    {
        public void UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName)
        {
            throw new System.NotImplementedException();
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
    }
}