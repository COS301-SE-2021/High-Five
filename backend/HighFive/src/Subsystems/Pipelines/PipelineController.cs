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
            throw new System.NotImplementedException();
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
            var pipelines = _pipelineService.GetPipelines();
            throw new System.NotImplementedException();
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}