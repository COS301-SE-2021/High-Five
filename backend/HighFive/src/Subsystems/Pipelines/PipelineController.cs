﻿using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    //[Authorize]
    public class PipelineController: PipelinesApiController
    {
        private readonly IPipelineService _pipelineService;
        private bool _baseContainerSet;
        
        public PipelineController(IPipelineService pipelineService)
        {
            _pipelineService = pipelineService;
            _baseContainerSet = false;
        }
        
        public override IActionResult AddTools(AddToolsRequest addToolsRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
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
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.CreatePipeline(createPipelineRequest).Result;
            return StatusCode(200, response);
        }

        public override IActionResult DeletePipeline(DeletePipelineRequest deletePipelineRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject() {Success = true};
            if (_pipelineService.DeletePipeline(deletePipelineRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Pipeline could not be deleted";
            return StatusCode(400, response);
        }

        public override IActionResult GetAllTools()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetAllTools();
            return StatusCode(200, response);
        }

        public override IActionResult GetPipeline(GetPipelineRequest getPipelineRequest)
        {
            throw new NotImplementedException();
        }

        public override IActionResult GetPipelineIds()
        {
            throw new NotImplementedException();
        }

        public override IActionResult GetPipelines()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetPipelines();
            return StatusCode(200, response);
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject {Success = true};
            if (_pipelineService.RemoveTools(removeToolsRequest).Result)
            {
                return StatusCode(200, response);
            }
            
            response.Success = false;
            response.Message = "Removal of tools from pipeline failed";
            return StatusCode(400, response);
        }
        
        private void ConfigureStorageManager()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                _pipelineService.SetBaseContainer("demo2"); // This line of code is for contingency's sake, to not break code still working on the old Storage system.
                //TODO: Remove above code when front-end is compatible with new storage structure.
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            _pipelineService.SetBaseContainer(jsonToken.Subject);
            _baseContainerSet = true;
        }
    }
}