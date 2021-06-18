using System;
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
        }
        
        public override IActionResult AddTools(AddToolsRequest addToolsRequest)
        {
            var response = new EmptyObject {Success = true};
            if (_pipelineService.AddTools(addToolsRequest))
            {
                return StatusCode(200, response);
            }

            response.Success = false;
            response.Message = "Addition of tools to pipeline failed";
            return StatusCode(400, response);
        }

        public override IActionResult CreatePipeline(CreatePipelineRequest createPipelineRequest)
        {
            _pipelineService.CreatePipeline(createPipelineRequest);
            var response = new EmptyObject {Success = true};
            return StatusCode(200, response);
        }

        public override IActionResult DeletePipeline(DeletePipelineRequest deletePipelineRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult GetPipelines()
        {
            var response = _pipelineService.GetPipelines();
            return StatusCode(200, response);
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            var response = new EmptyObject {Success = true};
            _pipelineService.RemoveTools(removeToolsRequest);
            return StatusCode(200, response);
        }
    }
}