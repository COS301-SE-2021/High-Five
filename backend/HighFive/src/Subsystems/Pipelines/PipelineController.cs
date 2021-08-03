using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    public class PipelineController: PipelinesApiController
    {
        private readonly IPipelineService _pipelineService;
        
        public PipelineController(IPipelineService pipelineService)
        {
            _pipelineService = pipelineService;
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            pipelineService.SetBaseContainer(jsonToken.Subject);
        }
        
        public override IActionResult AddTools(AddToolsRequest addToolsRequest)
        {
            var response = new EmptyObject {Success = true};
            if (_pipelineService.AddTools(addToolsRequest).Result)
            {
                return StatusCode(200, response);
            }

            response.Success = false;
            response.Message = "Addition of tools to pipeline failed";
            return StatusCode(400, response);
        }

        public override IActionResult CreatePipeline(CreatePipelineRequest createPipelineRequest)
        {
            var response = _pipelineService.CreatePipeline(createPipelineRequest).Result;
            return StatusCode(200, response);
        }

        public override IActionResult DeletePipeline(DeletePipelineRequest deletePipelineRequest)
        {
            var response = new EmptyObject() {Success = true};
            if (_pipelineService.DeletePipeline(deletePipelineRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Pipeline could not be deleted";
            return StatusCode(400, response);
        }

        public override IActionResult GetAllTools()
        {
            var response = _pipelineService.GetAllTools();
            return StatusCode(200, response);
        }

        public override IActionResult GetPipelines()
        {
            var response = _pipelineService.GetPipelines();
            return StatusCode(200, response);
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            var response = new EmptyObject {Success = true};
            if (_pipelineService.RemoveTools(removeToolsRequest).Result)
            {
                return StatusCode(200, response);
            }
            
            response.Success = false;
            response.Message = "Removal of tools from pipeline failed";
            return StatusCode(400, response);
        }
    }
}