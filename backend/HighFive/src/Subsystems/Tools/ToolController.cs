using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    public class ToolController: ToolsApiController
    {
        private readonly IToolService _toolService;
        private bool _baseContainerSet;

        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
            _baseContainerSet = false;
        }
        
        public override IActionResult DeleteTool(DeleteToolRequest deleteToolRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            throw new System.NotImplementedException();
        }

        public override IActionResult GetTools()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            var toolsList = _toolService.GetAllTools();
            var response = new GetAllToolsResponse {Tools = toolsList};
            return StatusCode(200, response);
        }

        public override IActionResult UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var status =_toolService.UploadAnalysisTool(sourceCode, model, toolName).Result;
            var response = new EmptyObject {Success = status};
            if (!status)
            {
                response.Message = "A tool with that name already exists.";
            }
            return StatusCode(200, response);
        }

        public override IActionResult UploadDrawingTool(IFormFile sourceCode, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var status =_toolService.UploadDrawingTool(sourceCode, toolName).Result;
            var response = new EmptyObject {Success = status};
            if (!status)
            {
                response.Message = "A tool with that name already exists.";
            }
            return StatusCode(200, response);
        }
        
        private void ConfigureStorageManager()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var alreadyExisted = _toolService.SetBaseContainer(jsonToken.Subject);
            if (!alreadyExisted)
            {
                var id = jsonToken.Subject;
                var displayName = jsonToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                var email = jsonToken.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
                _toolService.StoreUserInfo(id,displayName,email);
            }
            _baseContainerSet = true;
        }
    }
}