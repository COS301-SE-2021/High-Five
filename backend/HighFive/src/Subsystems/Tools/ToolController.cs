using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Tools
{
    [Authorize]
    public class ToolController: ToolsApiController
    {
        private readonly IToolService _toolService;
        private bool _baseContainerSet;

        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
            _baseContainerSet = false;
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult ApproveTool(ReviewToolRequest reviewToolRequest)
        {
            throw new NotImplementedException();
        }

        public override IActionResult CreateMetaDataType(string name, IFormFile file)
        {
            return StatusCode(503, null);
            /* if (!_baseContainerSet)
             {
                 ConfigureStorageManager();
             }
             var response = new EmptyObject
             {
                 Success = _toolService.CreateMetaDataType(file, name).Result
             };
             if (!response.Success)
             {
                 response.Message = "A metadata object with that name already exists.";
             }
             return StatusCode(200, response);*/
        }


        public override IActionResult DeleteTool(DeleteToolRequest deleteToolRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            var status = _toolService.DeleteTool(deleteToolRequest).Result;
            var response = new EmptyObject {Success = status};
            if (!status)
            {
                response.Message = "That tool does not exist.";
            }
            return StatusCode(200, response);
        }

        public override IActionResult GetMetaDataTypes()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _toolService.GetMetaDataTypes();
            return StatusCode(200, response);
        }

        public override IActionResult GetToolFiles(GetToolFilesRequest getToolFilesRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            GetToolFilesResponse response;
            try
            {
                response = _toolService.GetToolFiles(getToolFilesRequest);
            }
            catch (Exception)
            {
                return StatusCode(400, null);
            }

            return StatusCode(200, response);
        }

        public override IActionResult GetToolTypes()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new GetToolTypesResponse
            {
                ToolTypes = _toolService.GetToolTypes()
            };
            return StatusCode(200, response);
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

        [Authorize(Policy = "Admin")]
        public override IActionResult GetUnreviewedTools()
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = "Admin")]
        public override IActionResult RejectTool(ReviewToolRequest reviewToolRequest)
        {
            throw new NotImplementedException();
        }

        public override IActionResult UploadAnalysisTool(IFormFile sourceCode, IFormFile model, string metadataType, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            Tool tool;
            try
            {
                tool = _toolService.UploadAnalysisTool(sourceCode, model, metadataType, toolName).Result;
            }
            catch (Exception e)
            {
                return StatusCode(400, new EmptyObject {Success = false, Message = "Your uploaded dll has errors."});
            }

            if (tool == null)
            {
                return StatusCode(400, new EmptyObject{Success = false, Message = "A tool with that name already exists."});
            }
            return StatusCode(200, tool);
        }

        public override IActionResult UploadDrawingTool(IFormFile sourceCode, string metadataType, string toolName)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            Tool tool;
            try
            {
                tool = _toolService.UploadDrawingTool(sourceCode, metadataType, toolName).Result;
            }
            catch (Exception e)
            {
                return StatusCode(400, new EmptyObject {Success = false, Message = "Your uploaded dll has errors."});
            }

            if (tool == null)
            {
                return StatusCode(400, new EmptyObject{Success = false, Message = "A tool with that name already exists."});
            }
            return StatusCode(200, tool);
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
            var id = jsonToken.Subject;
            var displayName = jsonToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
            _toolService.StoreUserInfo(id,displayName,email);
            _baseContainerSet = true;
        }
    }
}