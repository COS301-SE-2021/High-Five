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
            throw new System.NotImplementedException();
        }

        public override IActionResult UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var status =_toolService.UploadAnalysisTool(sourceCode, model, toolName).Result;
            return StatusCode(200, new EmptyObject {Success = status});
        }

        public override IActionResult UploadDrawingTool(IFormFile sourceCode, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            throw new System.NotImplementedException();
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