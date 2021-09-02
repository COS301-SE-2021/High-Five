using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public class ToolController: ToolsApiController
    {
        public override IActionResult DeleteTool(DeleteToolRequest deleteToolRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult GetTools()
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult UploadDrawingTool(IFormFile sourceCode, string toolName)
        {
            throw new System.NotImplementedException();
        }
    }
}